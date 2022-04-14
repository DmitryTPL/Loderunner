using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class CharacterFallObserver : ICharacterFallObserver, IDisposable, ICharacterFilter
    {
        private float _leftFallPoint;
        private float _rightFallPoint;
        private float _bottomFallPoint;
        private float _fallPoint;
        private float _floorPoint;
        
        private readonly CancellationTokenSource _unsubscribeTokenSource = new();
        private readonly HashSet<int> _enteredGroundColliders = new();

        private bool IsFalling => !IsGrounded && !IsOnLadder && !IsOnCrossbar;
        
        public int CharacterId { get; set; }
        public bool IsGrounded => _enteredGroundColliders.Count > 0;
        public bool IsOnLadder { get; private set; }
        public bool IsOnCrossbar { get; private set; }

        public CharacterFallObserver(IAsyncEnumerableReceiver receiver)
        {
            receiver.Receive<ReachedSideToFallMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnReachedSideToFall).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<MovedAwayFromSideToFallMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnMoveAwayFromSideToFall).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<GotOffTheFloorMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(GotOffTheFloor).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<FloorReachedMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnFloorReached).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<EnterLadderMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnEnterLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitLadderMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnExitLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<EnterCrossbarMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnEnterCrossbar).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitCrossbarMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnExitCrossbar).AddTo(_unsubscribeTokenSource.Token);
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
            data.FallPoint = _fallPoint;
            data.FloorPoint = _floorPoint;
        }

        private void OnReachedSideToFall(ReachedSideToFallMessage message)
        {
            if (!IsGrounded && !IsOnLadder && !IsOnCrossbar)
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
            IsOnLadder = true;
        }

        private void OnExitLadder(ExitLadderMessage obj)
        {
            IsOnLadder = false;

            TrySetFallPoint();
        }

        private void TrySetFallPoint()
        {
            if (IsGrounded || IsOnLadder)
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
            IsOnCrossbar = true;
        }

        private void OnExitCrossbar(ExitCrossbarMessage obj)
        {
            IsOnCrossbar = false;
        }
    }
}