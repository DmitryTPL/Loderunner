using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class LevelExitPresenter : Presenter
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public LevelExitPresenter(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }

        public void PlayerReachedLevelExit()
        {
            _publisher.Publish(new PlayerReachedLevelExitMessage());
        }
    }
}