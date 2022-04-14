using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class LadderPresenterFactory : FactoryBase<LadderPresenter>
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public LadderPresenterFactory(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }
        protected override LadderPresenter CreateEntryWithDependencies()
        {
            return new LadderPresenter(_publisher);
        }
    }
}