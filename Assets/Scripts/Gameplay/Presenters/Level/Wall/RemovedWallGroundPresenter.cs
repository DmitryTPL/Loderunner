using Cysharp.Threading.Tasks;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class RemovedWallGroundPresenter : Presenter
    {
        private readonly IAsyncEnumerablePublisher _publisher;
        private readonly AsyncReactiveProperty<bool> _isActive = new(false);

        public IReadOnlyAsyncReactiveProperty<bool> IsActive => _isActive;

        public RemovedWallGroundPresenter(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }

        public void ChangeActivity(bool isActive)
        {
            _isActive.Value = isActive;
        }

        public void GroundReached(int characterId, float top)
        {
            _publisher.Publish(new RemovedWallGuardianGroundReachedMessage(characterId, Guid, top));
        }

        public void GotOffTheGround(int characterId)
        {
            _publisher.Publish(new GotOffTheRemovedWallGuardianGroundMessage(characterId, Guid));
        }
    }
}