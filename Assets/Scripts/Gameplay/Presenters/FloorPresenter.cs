using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class FloorPresenter : Presenter
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        private float _leftFallPoint;
        private float _rightFallPoint;

        public FloorPresenter(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }

        public void FloorReached(ICharacterInfo character, int floorId)
        {
            _publisher.Publish(new FloorReachedMessage(character.CharacterId, floorId));
        }

        public void GotOffTheFloor(ICharacterInfo character, int floorId)
        {
            _publisher.Publish(new GotOffTheFloorMessage(character.CharacterId, floorId));
        }
    }
}