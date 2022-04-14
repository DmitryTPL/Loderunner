using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class BorderPresenterFactory : FactoryBase<BorderPresenter>
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public BorderPresenterFactory(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }
        
        protected override BorderPresenter CreateEntryWithDependencies()
        {
            return new BorderPresenter(_publisher);
        }
    }
}