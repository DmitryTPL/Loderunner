using Loderunner.Service;
using UniTaskPubSub;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LadderPresenter : Presenter
    {
        private readonly IAsyncPublisher _publisher;

        public LadderPresenter(IAsyncPublisher publisher)
        {
            _publisher = publisher;
        }

        public void PlayerEnterLadder(Vector3 ladderBottomCenter, Vector3 ladderTop)
        {
            _publisher.PublishAsync(new PlayerEnterLadderMessage(ladderBottomCenter, ladderTop));
        }
        
        public void PlayerExitLadder()
        {
            _publisher.PublishAsync(new PlayerExitLadderMessage());
        }
    }
}