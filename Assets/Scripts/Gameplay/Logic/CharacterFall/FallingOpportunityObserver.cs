using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class FallingOpportunityObserver : IFallPointHolder, IDisposable, ICharacterFilter, IFloorPointHolder
    {
        private float _leftFallPoint;
        private float _rightFallPoint;
        private float _bottomFallPoint;
        private bool _isOnLadder;
        private bool _isOnCrossbar;
        private float _fallPoint;
        private float _floorPoint;
        
        private readonly CancellationTokenSource _unsubscribeTokenSource = new();
        private readonly HashSet<int> _enteredGroundColliders = new();

        public float FallPoint => _fallPoint;
        public float FloorPoint => _floorPoint;
        public Func<int, bool> CharacterFilter { get; set; }
        public bool IsGrounded => _enteredGroundColliders.Count > 0;

        public bool IsFalling => !IsGrounded && !_isOnLadder && !_isOnCrossbar;

        public FallingOpportunityObserver(IAsyncEnumerableReceiver receiver)
        {
            receiver.Receive<ReachedSideToFallMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnReachedSideToFall).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<MovedAwayFromSideToFallMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnMoveAwayFromSideToFall).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<GotOffTheFloorMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(GotOffTheFloor).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<FloorReachedMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnFloorReached).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<EnterLadderMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnEnterLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitLadderMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnExitLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<EnterCrossbarMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnEnterCrossbar).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitCrossbarMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnExitCrossbar).AddTo(_unsubscribeTokenSource.Token);
        }

        public void Dispose()
        {
            _unsubscribeTokenSource?.Dispose();
        }
        
        public void BeginToFallFromCrossbar(float characterPositionY)
        {
            _fallPoint = characterPositionY;
        }

        public void UpdateFallData(IFallStateData data)
        {
            data.IsGrounded = IsGrounded;
            data.FallPoint = FallPoint;
        }

        public void UpdateFloorData(IFloorData data)
        {
            data.FloorPoint = FloorPoint;
        }

        private void OnReachedSideToFall(ReachedSideToFallMessage message)
        {
            if (!IsGrounded && !_isOnLadder && !_isOnCrossbar)
            {
                return;
            }

            switch (message.SideToFall)
            {
                case SideToFallType.Left:
                    _leftFallPoint = message.FallPoint;
                    break;
                case SideToFallType.Right:
                    _rightFallPoint = message.FallPoint;
                    break;
                case SideToFallType.Bottom:
                    _bottomFallPoint = message.FallPoint;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnMoveAwayFromSideToFall(MovedAwayFromSideToFallMessage message)
        {
            switch (message.SideToFall)
            {
                case SideToFallType.Left:
                    _leftFallPoint = 0;
                    break;
                case SideToFallType.Right:
                    _rightFallPoint = 0;
                    break;
                case SideToFallType.Bottom:
                    _bottomFallPoint = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GotOffTheFloor(GotOffTheFloorMessage message)
        {
            _enteredGroundColliders.Remove(message.FloorId);

            TrySetFallPoint();
        }

        private void OnFloorReached(FloorReachedMessage message)
        {
            if (IsFalling)
            {
                _fallPoint = 0;
            }

            if (!IsGrounded)
            {
                _floorPoint = message.FloorPoint;
            }

            _enteredGroundColliders.Add(message.FloorId);
        }

        private void OnEnterLadder(EnterLadderMessage obj)
        {
            _isOnLadder = true;
        }

        private void OnExitLadder(ExitLadderMessage obj)
        {
            _isOnLadder = false;

            TrySetFallPoint();
        }

        private void TrySetFallPoint()
        {
            if (IsGrounded || _isOnLadder)
            {
                return;
            }
            
            if (Math.Abs(_rightFallPoint) > 0)
            {
                _fallPoint = _rightFallPoint;
            }
            else if (Math.Abs(_leftFallPoint) > 0)
            {
                _fallPoint = _leftFallPoint;
            }
            else if (Math.Abs(_bottomFallPoint) > 0)
            {
                _fallPoint = _bottomFallPoint;
            }
        }

        private void OnEnterCrossbar(EnterCrossbarMessage obj)
        {
            _isOnCrossbar = true;
        }

        private void OnExitCrossbar(ExitCrossbarMessage obj)
        {
            _isOnCrossbar = false;
        }
    }
}