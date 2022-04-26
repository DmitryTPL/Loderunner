using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class RemovedWallPresenterFactory : FactoryBase<RemovedWallPresenter>
    {
        private readonly GameConfig _gameConfig;
        private readonly IAsyncEnumerablePublisher _publisher;

        public RemovedWallPresenterFactory(GameConfig gameConfig, IAsyncEnumerablePublisher publisher)
        {
            _gameConfig = gameConfig;
            _publisher = publisher;
        }
        protected override RemovedWallPresenter CreateEntryWithDependencies()
        {
            return new RemovedWallPresenter(_gameConfig, _publisher);
        }
    }
}