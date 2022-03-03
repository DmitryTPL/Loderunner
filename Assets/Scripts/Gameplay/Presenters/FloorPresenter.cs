using Loderunner.Service;
using UniTaskPubSub;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class FloorPresenter : Presenter
    {
        private readonly IAsyncPublisher _publisher;

        private float _leftFallPoint;
        private float _rightFallPoint;

        public FloorPresenter(IAsyncPublisher publisher)
        {
            _publisher = publisher;
        }

        public void FloorReached(ICharacterView characterView, int colliderId)
        {
            _publisher.PublishAsync(new FloorReachedMessage(characterView, colliderId));
        }

        public void GotOffTheFloor(ICharacterView characterView, int colliderId)
        {
            _publisher.PublishAsync(new GotOffTheFloorMessage(characterView, colliderId));
        }
    }
}