using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class WallBlockPresenter : Presenter, IRemovableWallBlock
    {
        private readonly GameConfig _gameConfig;
        private readonly WallBlockRemoveConfig _wallBlockRemoveConfig;
        private readonly IAsyncEnumerablePublisher _publisher;
        private WallBlockLifeState _currentWallBlockLifeState;
        private Vector2 _position;

        public event Action BlockRemoveBegan;
        public event Action BlockRestoreBegan;
        public event Action BlockRestoreCompleted;

        public WallBlockPresenter(GameConfig gameConfig, WallBlockRemoveConfig wallBlockRemoveConfig,
            IAsyncEnumerablePublisher publisher)
        {
            _gameConfig = gameConfig;
            _wallBlockRemoveConfig = wallBlockRemoveConfig;
            _publisher = publisher;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public bool IsCharacterInBorders(Vector2 characterPosition)
        {
            return _position.x <= characterPosition.x && _position.x + _gameConfig.CellSize > characterPosition.x;
        }

        public IUniTaskAsyncEnumerable<WallBlockLifeState> TryRemove(int removerId)
        {
            return UniTaskAsyncEnumerable.Create<WallBlockLifeState>(async (writer, token) =>
            {
                if (_currentWallBlockLifeState is not (WallBlockLifeState.Restored or WallBlockLifeState.None))
                {
                    await writer.YieldAsync(WallBlockLifeState.None);
                    return;
                }

                await writer.YieldAsync(_currentWallBlockLifeState = BeginRemoveBlock(removerId));
                await writer.YieldAsync(_currentWallBlockLifeState = await WaitForBlockRemoved());
                await writer.YieldAsync(_currentWallBlockLifeState = await WaitToBeginRestoreBlock());
                await writer.YieldAsync(_currentWallBlockLifeState = await WaitForBlockRestored());
            });
        }

        private WallBlockLifeState BeginRemoveBlock(int removerId)
        {
            BlockRemoveBegan?.Invoke();

            _publisher.Publish(new WallBlockRemovingBeganMessage(removerId, _position));

            return WallBlockLifeState.Removing;
        }

        private async UniTask<WallBlockLifeState> WaitForBlockRemoved()
        {
            await UniTask.Delay(_wallBlockRemoveConfig.RemoveTime.ToMilliseconds());

            return WallBlockLifeState.Removed;
        }

        private async UniTask<WallBlockLifeState> WaitToBeginRestoreBlock()
        {
            await UniTask.Delay(_wallBlockRemoveConfig.RemovedStateTime.ToMilliseconds());

            BlockRestoreBegan?.Invoke();

            return WallBlockLifeState.Restoring;
        }

        private async UniTask<WallBlockLifeState> WaitForBlockRestored()
        {
            await UniTask.Delay(_wallBlockRemoveConfig.RestoreTime.ToMilliseconds());

            BlockRestoreCompleted?.Invoke();

            return WallBlockLifeState.Restored;
        }
    }
}