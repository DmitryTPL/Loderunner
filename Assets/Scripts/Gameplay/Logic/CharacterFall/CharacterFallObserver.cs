using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class CharacterFallObserver : ICharacterFallObserver
    {
        private int _characterId;
        private float _leftFallPoint;
        private float _rightFallPoint;
        private float _bottomFallPoint;
        private float _fallPoint;
        private float _floorPoint;
        
        private readonly CancellationTokenSource _unsubscribeTokenSource = new();
        private readonly HashSet<Guid> _enteredGroundColliders = new();

        private bool IsFalling => !IsGrounded && !IsOnLadder && !IsOnCrossbar;
        
        public bool IsGrounded => _enteredGroundColliders.Count > 0;
        public bool IsOnLadder { get; private set; }
        public bool IsOnCrossbar { get; private set; }

        public CharacterFallObserver(IAsyncEnumerableReceiver receiver)
        {
            receiver.Receive<ReachedSideToFallMessage>().Where(m => m.IsCharacterMatch(_characterId)).Subscribe(OnReachedSideToFall).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<MovedAwayFromSideToFallMessage>().Where(m => m.IsCharacterMatch(_characterId)).Subscribe(OnMoveAwayFromSideToFall).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<GotOffTheFloorMessage>().Where(m => m.IsCharacterMatch(_characterId)).Subscribe(GotOffTheFloor).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<FloorReachedMessage>().Where(m => m.IsCharacterMatch(_characterId)).Subscribe(OnFloorReached).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<EnterLadderMessage>().Where(m => m.IsCharacterMatch(_characterId)).Subscribe(OnEnterLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitLadderMessage>().Where(m => m.IsCharacterMatch(_characterId)).Subscribe(OnExitLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<EnterCrossbarMessage>().Where(m => m.IsCharacterMatch(_characterId)).Subscribe(OnEnterCrossbar).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitCrossbarMessage>().Where(m => m.IsCharacterMatch(_characterId)).Subscribe(OnExitCrossbar).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<CharacterNeedToFallInRemovedBlockMessage>().Where(m => m.IsCharacterMatch(_characterId)).Subscribe(OnCharacterNeedToFallInRemovedBlock).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<BorderReachedMessage>().Where(m => m.IsCharacterMatch(_characterId) && m.Border == BorderType.Bottom).Subscribe(OnBottomBorderReached).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<MovedAwayFromBorderMessage>().Where(m => m.IsCharacterMatch(_characterId) && m.Border == BorderType.Bottom).Subscribe(OnMoveAwayFromBorder).AddTo(_unsubscribeTokenSource.Token);
        }

        public void Dispose()
        {
            _unsubscribeTokenSource?.Dispose();
        }

        public void BindCharacter(int id)
        {
            _characterId = id;
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
                _floorPoint = message.TopPoint;
            }

            _enteredGroundColliders.Add(message.FloorId);
        }

        private void OnBottomBorderReached(BorderReachedMessage message)
        {
            if (!IsGrounded)
            {
                _floorPoint = message.TopPoint;
            }
            
            _enteredGroundColliders.Add(message.BorderId);
        }

        private void OnMoveAwayFromBorder(MovedAwayFromBorderMessage message)
        {
            _enteredGroundColliders.Remove(message.BorderId);
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

        private void OnCharacterNeedToFallInRemovedBlock(CharacterNeedToFallInRemovedBlockMessage message)
        {
            _fallPoint = message.FallPoint;
            _enteredGroundColliders.Clear();
        }
    }
}