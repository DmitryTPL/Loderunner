using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class SideToFallPresenterFactory: FactoryBase<SideToFallPresenter>
    {
        private readonly IAsyncEnumerablePublisher _publisher;
        private readonly GameConfig _gameConfig;

        public SideToFallPresenterFactory(IAsyncEnumerablePublisher publisher, GameConfig gameConfig)
        {
            _publisher = publisher;
            _gameConfig = gameConfig;
        }
        
        protected override SideToFallPresenter CreateEntryWithDependencies()
        {
            return new SideToFallPresenter(_publisher, _gameConfig);
        }
    }
}