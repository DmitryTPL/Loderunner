using System.Threading;
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
        private readonly AsyncReactiveProperty<WallBlockLifeState> _currentWallBlockLifeState = new (WallBlockLifeState.None);
        
        private Vector2 _position;
        private RemovedWallPresenter _removedWallPresenter;

        public IReadOnlyAsyncReactiveProperty<WallBlockLifeState> CurrentWallBlockLifeState => _currentWallBlockLifeState;

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

        public void SetRemovedWallPresenter(RemovedWallPresenter removedWallPresenter)
        {
            _removedWallPresenter = removedWallPresenter;
        } 

        public bool IsCharacterInBorders(Vector2 characterPosition)
        {
            return _position.x <= characterPosition.x && _position.x + _gameConfig.CellSize > characterPosition.x;
        }

        public IUniTaskAsyncEnumerable<WallBlockLifeState> TryRemove(int removerId)
        {
            return UniTaskAsyncEnumerable.Create<WallBlockLifeState>(async (writer, token) =>
            {
                if (_currentWallBlockLifeState.Value is not (WallBlockLifeState.Restored or WallBlockLifeState.None) || token.IsCancellationRequested)
                {
                    await writer.YieldAsync(WallBlockLifeState.None);
                    return;
                }

                await writer.YieldAsync(_currentWallBlockLifeState.Value = BeginRemoveBlock(removerId));
                await writer.YieldAsync(_currentWallBlockLifeState.Value = await WaitForBlockRemoved(token));
                await writer.YieldAsync(_currentWallBlockLifeState.Value = await WaitToBeginRestoreBlock(token));
                await writer.YieldAsync(_currentWallBlockLifeState.Value = await WaitForBlockRestored(token));
            });
        }

        private WallBlockLifeState BeginRemoveBlock(int removerId)
        {
            _publisher.Publish(new WallBlockRemovingBeganMessage(removerId, _position));

            return WallBlockLifeState.Removing;
        }

        private async UniTask<WallBlockLifeState> WaitForBlockRemoved(CancellationToken cancellationToken)
        {
            await UniTask.Delay(_wallBlockRemoveConfig.RemoveTime.ToMilliseconds(), cancellationToken: cancellationToken);

            _removedWallPresenter.Activate();
            
            return WallBlockLifeState.Removed;
        }

        private async UniTask<WallBlockLifeState> WaitToBeginRestoreBlock(CancellationToken cancellationToken)
        {
            await UniTask.Delay(_wallBlockRemoveConfig.RemovedStateTime.ToMilliseconds(), cancellationToken: cancellationToken);

            return WallBlockLifeState.Restoring;
        }

        private async UniTask<WallBlockLifeState> WaitForBlockRestored(CancellationToken cancellationToken)
        {
            _publisher.Publish(new WallBlockRestoringBeganMessage(_position));
            
            await UniTask.Delay(_wallBlockRemoveConfig.RestoreTime.ToMilliseconds(), cancellationToken: cancellationToken);
            
            _removedWallPresenter.Deactivate();

            return WallBlockLifeState.Restored;
        }
    }
}