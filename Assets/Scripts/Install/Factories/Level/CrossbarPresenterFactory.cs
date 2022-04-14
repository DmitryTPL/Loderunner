using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class CrossbarPresenterFactory: FactoryBase<CrossbarPresenter>
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public CrossbarPresenterFactory(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }
        
        protected override CrossbarPresenter CreateEntryWithDependencies()
        {
            return new CrossbarPresenter(_publisher);
        }
    }
}