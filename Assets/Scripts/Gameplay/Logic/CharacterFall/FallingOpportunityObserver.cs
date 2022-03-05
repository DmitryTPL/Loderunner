using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class FallingOpportunityObserver : IFallPointHolder, IDisposable
    {
        private float _leftFallPoint;
        private float _rightFallPoint;
        private float _bottomFallPoint;
        private bool _isOnLadder;
        private CancellationTokenSource _unsubscribeTokenSource = new();
        private HashSet<int> _enteredGroundColliders = new();

        public float FallPoint
        {
            get
            {
                var fallingPosition = 0f;

                if (Math.Abs(_rightFallPoint) > 0)
                {
                    fallingPosition = _rightFallPoint;
                }
                else if (Math.Abs(_leftFallPoint) > 0)
                {
                    fallingPosition = _leftFallPoint;
                }
                else if (Math.Abs(_bottomFallPoint) > 0)
                {
                    fallingPosition = _bottomFallPoint;
                }

                return fallingPosition;
            }
        }

        public Func<int, bool> CharacterFilter { get; set; }
        public bool IsGrounded => _enteredGroundColliders.Count > 0;

        public FallingOpportunityObserver(IAsyncEnumerableReceiver receiver)
        {
            receiver.Receive<ReachedSideToFallMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnReachedSideToFall).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<MovedAwayFromSideToFallMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnMoveAwayFromSideToFall).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<GotOffTheFloorMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(GotOffTheFloor).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<FloorReachedMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnFloorReached).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<EnterLadderMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnEnterLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitLadderMessage>().Where(m => CharacterFilter(m.CharacterId)).Subscribe(OnExitLadder).AddTo(_unsubscribeTokenSource.Token);
        }

        private void OnReachedSideToFall(ReachedSideToFallMessage message)
        {
            if (!IsGrounded && !_isOnLadder)
            {
                return;
            }
            
            switch (message.SideToFall)
            {
                case BorderType.Left:
                    _leftFallPoint = message.FallPoint;
                    break;
                case BorderType.Right:
                    _rightFallPoint = message.FallPoint;
                    break;
                case BorderType.Bottom:
                    _bottomFallPoint = message.FallPoint;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnMoveAwayFromSideToFall(MovedAwayFromSideToFallMessage message)
        {
            if (!IsGrounded && !_isOnLadder)
            {
                return;
            }
            
            switch (message.SideToFall)
            {
                case BorderType.Left:
                    _leftFallPoint = 0;
                    break;
                case BorderType.Right:
                    _rightFallPoint = 0;
                    break;
                case BorderType.Bottom:
                    _bottomFallPoint = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GotOffTheFloor(GotOffTheFloorMessage message)
        {
            _enteredGroundColliders.Remove(message.FloorId);
        }

        private void OnFloorReached(FloorReachedMessage message)
        {
            if (!IsGrounded && !_isOnLadder)
            {
                _rightFallPoint = 0;
                _leftFallPoint = 0;
                _bottomFallPoint = 0;
            }
            
            _enteredGroundColliders.Add(message.FloorId);
        }

        public void Dispose()
        {
            _unsubscribeTokenSource?.Dispose();
        }

        private void OnEnterLadder(EnterLadderMessage obj)
        {
            _isOnLadder = true;
        }

        private void OnExitLadder(ExitLadderMessage obj)
        {
            _isOnLadder = false;
        }
    }
}