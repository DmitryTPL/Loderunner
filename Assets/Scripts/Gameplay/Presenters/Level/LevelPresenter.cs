using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelPresenter: Presenter
    {
        private readonly LevelData _levelData;
        private readonly IAsyncEnumerablePublisher _publisher;

        public LevelPresenter(LevelData levelData, IAsyncEnumerablePublisher publisher, IAsyncEnumerableReceiver receiver)
        {
            _levelData = levelData;
            _publisher = publisher;
            
            receiver.Receive<WallBlockRemovingBeganMessage>().Subscribe(OnWallBlockRemovingBegan).AddTo(DisposeCancellationToken);
            receiver.Receive<WallBlockRestoringBeganMessage>().Subscribe(OnWallBlockRestoringBegan).AddTo(DisposeCancellationToken);
        }

        public void SetCameraBounds(Bounds bounds)
        {
            _levelData.CameraBounds = bounds;
        }

        public void LevelCreated(int levelNumber, Matrix<int> map, LevelConfig levelConfig)
        {
            _levelData.LevelNumber = levelNumber;
            _levelData.Map = map;
            _levelData.Config = levelConfig;
            _publisher.Publish(new LevelCreatedMessage(levelNumber));
        }

        private void OnWallBlockRemovingBegan(WallBlockRemovingBeganMessage message)
        {
            var mapPosition = message.WallBlockPosition.ToVector2Int();

            _levelData.Map[mapPosition.y, mapPosition.x] = 1;
        }

        private void OnWallBlockRestoringBegan(WallBlockRestoringBeganMessage message)
        {
            var mapPosition = message.WallBlockPosition.ToVector2Int();

            _levelData.Map[mapPosition.y, mapPosition.x] = -1;
        }
    }
}