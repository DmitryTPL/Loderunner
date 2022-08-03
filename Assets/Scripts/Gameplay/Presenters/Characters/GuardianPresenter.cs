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
        public enum RemovedWallBlockState
        {
            None,
            Falling,
            Stuck,
            ClampedByTheWall,
            ClimbingUp
        }

        private static readonly Vector2Int _undefinedMapPosition = new(-1, -1);

        private readonly IGuardiansCommander _guardiansCommander;
        private readonly GuardianConfig _guardianConfig;
        private readonly AsyncReactiveProperty<RemovedWallBlockState> _currentRemovedWallBlockState = new(RemovedWallBlockState.None);

        private Stack<Vector2Int> _pathToPlayer = new();
        private Vector2Int _mapPosition = _undefinedMapPosition;
        private bool _hasGold;
        private readonly StateData _guardianStateData;

        public event Action Respawn;

        public override CharacterType CharacterType => CharacterType.Guardian;
        public IReadOnlyAsyncReactiveProperty<RemovedWallBlockState> CurrentRemovedWallBlockState => _currentRemovedWallBlockState;

        public GuardianPresenter(GuardianStateContext stateContext, IAsyncEnumerableReceiver receiver, IAsyncEnumerablePublisher publisher,
            ICharacterFallObserver characterFallObserver, IGuardiansCommander guardiansCommander, GuardianConfig guardianConfig)
            : base(stateContext, receiver, publisher, characterFallObserver, stateContext.StateData)
        {
            _guardiansCommander = guardiansCommander;
            _guardianConfig = guardianConfig;
            _guardianStateData = stateContext.StateData;

            receiver.Receive<UpdateGuardiansPathMessage>().Subscribe(OnUpdatePath).AddTo(DisposeCancellationToken);
            receiver.Receive<CharacterReachedGoldMessage>().Subscribe(OnGoldReached).AddTo(DisposeCancellationToken);
            receiver.Receive<CharacterNeedToFallInRemovedBlockMessage>().Where(m => m.IsCharacterMatch(Id)).Subscribe(OnCharacterNeedToFallInRemovedBlock)
                .AddTo(DisposeCancellationToken);
            receiver.Receive<WallBlockRestoringBeganMessage>().Subscribe(OnWallBlockRestoringBegan).AddTo(DisposeCancellationToken);
            receiver.Receive<WallBlockRestoredMessage>().Subscribe(OnWallBlockRestored).AddTo(DisposeCancellationToken);
            receiver.Receive<GuardianRespawnedMessage>().Where(m => m.IsCharacterMatch(Id)).Subscribe(OnRespawned).AddTo(DisposeCancellationToken);
        }

        public override void CharacterCreated(int id)
        {
            base.CharacterCreated(id);

            _guardiansCommander.Register(id);
        }

        public (int HorizontalDirection, int VerticalDirection) GetDirection()
        {
            if (_pathToPlayer.Count == 0 || 
                _currentRemovedWallBlockState.Value is RemovedWallBlockState.Stuck or RemovedWallBlockState.ClampedByTheWall)
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

        protected override void FinishClimbing()
        {
            base.FinishClimbing();

            if (_currentRemovedWallBlockState.Value == RemovedWallBlockState.ClimbingUp)
            {
                _guardianStateData.ClimbingData = new ClimbingData();
                _currentRemovedWallBlockState.Value = RemovedWallBlockState.None;
            }
        }

        private bool IsPointReached(Vector2Int direction, Vector2Int goalPosition)
        {
            var shiftedPosition = Position - new Vector2(0.5f, 0);

            var xCoordinateReached = (goalPosition.x - shiftedPosition.x) * direction.x < float.Epsilon;
            var yCoordinateReached = (goalPosition.y - shiftedPosition.y) * direction.y < float.Epsilon;

            return xCoordinateReached && yCoordinateReached;
        }

        private void OnUpdatePath(UpdateGuardiansPathMessage message)
        {
            UpdatePath().Forget();
        }

        private async UniTaskVoid UpdatePath()
        {
            if (_currentRemovedWallBlockState.Value is RemovedWallBlockState.Stuck or RemovedWallBlockState.ClampedByTheWall)
            {
                return;
            }

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
            LaunchRemovedWallBlockLifetime(message.FallPoint, message.Top).Forget();
        }

        private async UniTaskVoid LaunchRemovedWallBlockLifetime(float center, float climbFinishPoint)
        {
            _currentRemovedWallBlockState.Value = RemovedWallBlockState.Falling;

            DropGold();
            
            await UniTask.WaitWhile(() => !_characterFallObserver.IsGrounded);

            _currentRemovedWallBlockState.Value = RemovedWallBlockState.Stuck;

            await UniTask.Delay(_guardianConfig.StuckInRemovedBlockTimeout.ToMilliseconds());

            if (!CanAct)
            {
                return;
            }

            if (_currentRemovedWallBlockState != RemovedWallBlockState.ClampedByTheWall)
            {
                _currentRemovedWallBlockState.Value = RemovedWallBlockState.ClimbingUp;

                _mapPosition = Position.ToVector2Int();

                _guardianStateData.ClimbingData = new ClimbingData(0, center, climbFinishPoint);

                UpdatePath().Forget();
            }
        }

        private void DropGold()
        {
            if (_hasGold)
            {
                var currentPosition = Position.ToVector2Int();

                var dropPosition = new Vector2Int(currentPosition.x, currentPosition.y + 1);

                _publisher.Publish(new GuardianDropGoldMessage(dropPosition));
            }

            _hasGold = false;
        }

        private void Reset()
        {
            CanAct = false;
            
            _mapPosition = _undefinedMapPosition;
            _currentRemovedWallBlockState.Value = RemovedWallBlockState.None;
            _pathToPlayer = new Stack<Vector2Int>();
        }

        private void OnWallBlockRestoringBegan(WallBlockRestoringBeganMessage message)
        {
            if (_currentRemovedWallBlockState == RemovedWallBlockState.Stuck &&
                Position.ToVector2Int() == message.WallBlockPosition.ToVector2Int())
            {
                _currentRemovedWallBlockState.Value = RemovedWallBlockState.ClampedByTheWall;
            }
        }

        private void OnWallBlockRestored(WallBlockRestoredMessage message)
        {
            if (_currentRemovedWallBlockState == RemovedWallBlockState.ClampedByTheWall &&
                Position.ToVector2Int() == message.WallBlockPosition.ToVector2Int())
            {
                Reset();
                _publisher.Publish(new RespawnGuardianMessage(Id));
            }
        }

        private async UniTaskVoid OnRespawned(GuardianRespawnedMessage message)
        {
            Respawn?.Invoke();
            
            await UniTask.Delay(_guardianConfig.RespawnTimeout.ToMilliseconds());

            CanAct = true;
            
            UpdatePath().Forget();
        }
    }
}