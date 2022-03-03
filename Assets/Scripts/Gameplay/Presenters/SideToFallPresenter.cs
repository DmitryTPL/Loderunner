using Loderunner.Service;
using UniTaskPubSub;

namespace Loderunner.Gameplay
{
    public class SideToFallPresenter : Presenter
    {
        private readonly IAsyncPublisher _publisher;
        
        public GameConfig GameConfig { get; }

        public SideToFallPresenter(IAsyncPublisher publisher, GameConfig gameConfig)
        {
            _publisher = publisher;
            
            GameConfig = gameConfig;
        }

        public void ReachingSideToFall(ICharacterView characterView, float fallPoint, BorderType sideToFall)
        {
            _publisher.PublishAsync(new ReachedSideToFallMessage(characterView, fallPoint, sideToFall));
        }

        public void MoveAwayFromSideToFall(ICharacterView characterView, BorderType sideToFall)
        {
            _publisher.PublishAsync(new MovedAwayFromSideToFallMessage(characterView, sideToFall));
        }
    }
}