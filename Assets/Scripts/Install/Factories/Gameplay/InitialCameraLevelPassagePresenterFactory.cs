using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class InitialCameraLevelPassagePresenterFactory : FactoryBase<InitialCameraLevelPassagePresenter>
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public InitialCameraLevelPassagePresenterFactory(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }
        
        protected override InitialCameraLevelPassagePresenter CreateEntryWithDependencies()
        {
            return new InitialCameraLevelPassagePresenter(_publisher);
        }
    }
}