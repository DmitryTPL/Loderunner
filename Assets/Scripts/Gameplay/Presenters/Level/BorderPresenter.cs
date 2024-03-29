﻿using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class BorderPresenter: Presenter
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public BorderPresenter(IAsyncEnumerablePublisher publisher)
        {
            _publisher = publisher;
        }
        
        public void EnterBorderTrigger(ICharacterInfo character, BorderType border, float topPoint)
        {
            _publisher.Publish(new BorderReachedMessage(character.CharacterId, Guid, border, topPoint));
        }
        
        public void ExitBorderTrigger(ICharacterInfo character, BorderType border)
        {
            _publisher.Publish(new MovedAwayFromBorderMessage(character.CharacterId, Guid, border));
        }
    }
}