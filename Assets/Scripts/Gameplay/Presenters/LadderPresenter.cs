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

        public void CharacterEnterLadder(ICharacterView characterView, Vector3 ladderBottomCenter, Vector3 ladderTop)
        {
            _publisher.PublishAsync(new PlayerEnterLadderMessage(characterView, ladderBottomCenter, ladderTop));
        }
        
        public void PlayerExitLadder(ICharacterView characterView)
        {
            _publisher.PublishAsync(new PlayerExitLadderMessage(characterView));
        }
    }
}