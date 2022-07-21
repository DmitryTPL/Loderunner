using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LadderPresenter : Presenter
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public LadderPresenter(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }

        public void CharacterEnterLadder(ICharacterInfo character, Vector3 ladderBottomCenter, Vector3 ladderTop)
        {
            _publisher.Publish(new EnterLadderMessage(character.CharacterId, ladderBottomCenter, ladderTop));
        }
        
        public void CharacterExitLadder(ICharacterInfo character)
        {
            _publisher.Publish(new ExitLadderMessage(character.CharacterId));
        }
    }
}