using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class WallBlockPresenterFactory : FactoryBase<WallBlockPresenter>
    {
        private readonly GameConfig _gameConfig;
        private readonly WallBlockRemoveConfig _wallBlockRemoveConfig;
        private readonly IAsyncEnumerablePublisher _publisher;

        public WallBlockPresenterFactory(GameConfig gameConfig, WallBlockRemoveConfig wallBlockRemoveConfig, IAsyncEnumerablePublisher publisher)
        {
            _gameConfig = gameConfig;
            _wallBlockRemoveConfig = wallBlockRemoveConfig;
            _publisher = publisher;
        }
        protected override WallBlockPresenter CreateEntryWithDependencies()
        {
            return new WallBlockPresenter(_gameConfig, _wallBlockRemoveConfig, _publisher);
        }
    }
}