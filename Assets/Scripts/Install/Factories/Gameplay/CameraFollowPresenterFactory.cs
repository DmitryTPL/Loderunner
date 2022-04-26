using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class CameraFollowPresenterFactory : FactoryBase<CameraFollowPresenter>
    {
        private readonly IAsyncEnumerableReceiver _receiver;
        private readonly GameConfig _gameConfig;
        private readonly ILevelData _levelData;

        public CameraFollowPresenterFactory(IAsyncEnumerableReceiver receiver, GameConfig gameConfig, ILevelData levelData)
        {
            _receiver = receiver;
            _gameConfig = gameConfig;
            _levelData = levelData;
        }
        
        protected override CameraFollowPresenter CreateEntryWithDependencies()
        {
            return new CameraFollowPresenter(_receiver, _gameConfig, _levelData);
        }
    }
}