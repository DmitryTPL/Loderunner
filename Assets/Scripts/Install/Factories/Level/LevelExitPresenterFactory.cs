using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class LevelExitPresenterFactory : FactoryBase<LevelExitPresenter>
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public LevelExitPresenterFactory(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }
        
        protected override LevelExitPresenter CreateEntryWithDependencies()
        {
            return new LevelExitPresenter(_publisher);
        }
    }
}