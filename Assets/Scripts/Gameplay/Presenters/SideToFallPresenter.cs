using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class SideToFallPresenter : Presenter
    {
        private readonly IAsyncEnumerablePublisher _publisher;
        
        public GameConfig GameConfig { get; }

        public SideToFallPresenter(IAsyncEnumerablePublisher publisher, GameConfig gameConfig)
        {
            _publisher = publisher;
            
            GameConfig = gameConfig;
        }

        public void ReachingSideToFall(ICharacterInfo character, float fallPoint, SideToFallType sideToFall)
        {
            _publisher.Publish(new ReachedSideToFallMessage(character.CharacterId, fallPoint, sideToFall));
        }

        public void MoveAwayFromSideToFall(ICharacterInfo character, SideToFallType sideToFall)
        {
            _publisher.Publish(new MovedAwayFromSideToFallMessage(character.CharacterId, sideToFall));
        }
    }
}