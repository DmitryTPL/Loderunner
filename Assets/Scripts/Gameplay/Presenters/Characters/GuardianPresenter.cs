using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GuardianPresenter : CharacterPresenter
    {
        private enum RemovedWallBlockState
        {
            None,
            Stuck,
            ClampedByTheWall,
            ClimbingUp
        }

        private static readonly Vector2Int _undefinedMapPosition = new(-1, -1);

        private readonly IGuardiansCommander _guardiansCommander;
        private readonly GuardianConfig _guardianConfig;

        private Stack<Vector2Int> _pathToPlayer = new();
        private Vector2Int _mapPosition = _undefinedMapPosition;
        private bool _hasGold;
        private RemovedWallBlockState _removedWallBlockState;

        public override CharacterType CharacterType => CharacterType.Guardian;

        public GuardianPresenter(GuardianStateContext stateContext, IAsyncEnumerableReceiver receiver, IAsyncEnumerablePublisher publisher,
            ICharacterFallObserver characterFallObserver, IGuardiansCommander guardiansCommander, GuardianConfig guardianConfig)
            : base(stateContext, receiver, publisher, characterFallObserver, stateContext.StateData)
        {
            _guardiansCommander = guardiansCommander;
            _guardianConfig = guardianConfig;

            receiver.Receive<UpdateGuardiansPathMessage>().Subscribe(OnUpdatePath).AddTo(DisposeCancellationToken);
            receiver.Receive<CharacterReachedGoldMessage>().Subscribe(OnGoldReached).AddTo(DisposeCancellationToken);
            receiver.Receive<CharacterNeedToFallInRemovedBlockMessage>().Where(m => m.IsCharacterMatch(Id)).Subscribe(OnCharacterNeedToFallInRemovedBlock)
                .AddTo(DisposeCancellationToken);
            receiver.Receive<WallBlockRestoringBeganMessage>().Subscribe(OnWallBlockRestoringBegan).AddTo(DisposeCancellationToken);
        }

        public override void CharacterCreated(int id)
        {
            base.CharacterCreated(id);

            _guardiansCommander.Register(id);
        }

        public (int HorizontalDirection, int VerticalDirection) GetDirection()
        {
            if (_pathToPlayer.Count == 0)
            {
                return (0, 0);
            }

            var nextPointOnPath = _pathToPlayer.Peek();

            var direction = nextPointOnPath - _mapPosition;

            if (IsPointReached(direction, nextPointOnPath))
            {
                _pathToPlayer.Pop();

                if (_pathToPlayer.Count == 0)
                {
                    return (0, 0);
                }

                _mapPosition = nextPointOnPath;

                return (0, 0);
            }

            return (direction.x, direction.y);
        }

        public void PlayerCached()
        {
            _publisher.Publish(new PlayerCachedMessage());
        }

        private bool IsPointReached(Vector2Int direction, Vector2Int goalPosition)
        {
            var shiftedPosition = Position - new Vector2(0.5f, 0);

            var xCoordinateReached = (goalPosition.x - shiftedPosition.x) * direction.x < float.Epsilon;
            var yCoordinateReached = (goalPosition.y - shiftedPosition.y) * direction.y < float.Epsilon;

            return xCoordinateReached && yCoordinateReached;
        }

        private async UniTaskVoid OnUpdatePath(UpdateGuardiansPathMessage message)
        {
            if (_mapPosition == _undefinedMapPosition)
            {
                _mapPosition = Position.ToVector2Int();
            }

            var findPathResult = await _guardiansCommander.GetPath(Id, _mapPosition);

            if (findPathResult.SearchResult == SearchPathResult.Success)
            {
                _pathToPlayer = findPathResult.Path;
            }
            else
            {
                this.LogError($"Can't find path to player; guardian: {Id}");
            }
        }

        private void OnGoldReached(CharacterReachedGoldMessage message)
        {
            if (_hasGold)
            {
                return;
            }

            _hasGold = true;

            _publisher.Publish(new CharacterCollectGoldMessage(message.GoldGuid, Id));
        }

        private void OnCharacterNeedToFallInRemovedBlock(CharacterNeedToFallInRemovedBlockMessage message)
        {
            LaunchRemovedWallBlockLifetime().Forget();
        }

        private async UniTaskVoid LaunchRemovedWallBlockLifetime()
        {
            _removedWallBlockState = RemovedWallBlockState.Stuck;

            await UniTask.WaitWhile(() => !_characterFallObserver.IsGrounded);

            CanAct = false;

            await UniTask.Delay(_guardianConfig.StuckInRemovedBlockTimeout.ToMilliseconds());

            if (_removedWallBlockState != RemovedWallBlockState.ClampedByTheWall)
            {
                _removedWallBlockState = RemovedWallBlockState.ClimbingUp;
                CanAct = true;
            }
        }

        private void OnWallBlockRestoringBegan(WallBlockRestoringBeganMessage message)
        {
            if (_removedWallBlockState == RemovedWallBlockState.Stuck)
            {
                if (_mapPosition == message.WallBlockPosition.ToVector2Int())
                {
                    _removedWallBlockState = RemovedWallBlockState.ClampedByTheWall;
                }
            }
        }
    }
}