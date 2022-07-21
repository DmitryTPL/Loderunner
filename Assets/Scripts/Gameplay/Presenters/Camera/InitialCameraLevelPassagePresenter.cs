using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class InitialCameraLevelPassagePresenter : Presenter
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public InitialCameraLevelPassagePresenter(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }
        
        public void PassageCompleted()
        {
            _publisher.Publish(new GameStartedMessage());
        }
    }
}