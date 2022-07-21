using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class CrossbarPresenter : Presenter
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public CrossbarPresenter(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }
        
        public void CharacterEnterCrossbar(ICharacterInfo character, float leftPosition, float rightPosition, float center)
        {
            _publisher.Publish(new EnterCrossbarMessage(character.CharacterId, leftPosition, rightPosition, center));
        }

        public void PlayerExitCrossbar(ICharacterInfo character)
        {
            _publisher.Publish(new ExitCrossbarMessage(character.CharacterId));
        }
    }
}