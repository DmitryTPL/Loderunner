using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class FloorPresenterFactory : FactoryBase<FloorPresenter>
    {
        private readonly IAsyncEnumerablePublisher _publisher;
        private readonly GameConfig _gameConfig;

        public FloorPresenterFactory(IAsyncEnumerablePublisher publisher, GameConfig gameConfig)
        {
            _publisher = publisher;
            _gameConfig = gameConfig;
        }

        protected override FloorPresenter CreateEntryWithDependencies()
        {
            return new FloorPresenter(_publisher, _gameConfig);
        }
    }
}