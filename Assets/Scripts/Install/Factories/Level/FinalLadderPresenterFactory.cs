using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class FinalLadderPresenterFactory : FactoryBase<FinalLadderPresenter>
    {
        private readonly IAsyncEnumerableReceiver _receiver;

        public FinalLadderPresenterFactory(IAsyncEnumerableReceiver receiver)
        {
            _receiver = receiver;
        }

        protected override FinalLadderPresenter CreateEntryWithDependencies()
        {
            return new FinalLadderPresenter(_receiver);
        }
    }
}