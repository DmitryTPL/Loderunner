using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class GoldPresenterFactory : FactoryBase<GoldPresenter>
    {
        private readonly IAsyncEnumerablePublisher _publisher;
        private readonly IAsyncEnumerableReceiver _receiver;

        public GoldPresenterFactory(IAsyncEnumerablePublisher publisher, IAsyncEnumerableReceiver receiver)
        {
            _publisher = publisher;
            _receiver = receiver;
        }

        protected override GoldPresenter CreateEntryWithDependencies()
        {
            return new GoldPresenter(_publisher, _receiver);
        }
    }
}