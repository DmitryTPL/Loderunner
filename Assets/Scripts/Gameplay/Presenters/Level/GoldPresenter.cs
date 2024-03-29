﻿using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class GoldPresenter : Presenter
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public event Action GoldHasBeenTaken;

        public GoldPresenter(IAsyncEnumerablePublisher publisher, IAsyncEnumerableReceiver receiver)
        {
            _publisher = publisher;

            receiver.Receive<CharacterCollectGoldMessage>().Where(m => m.GoldGuid.Equals(Guid)).Subscribe(OnCharacterTookGold).AddTo(DisposeCancellationToken);
        }

        public void CharacterReachedGold(ICharacterInfo character)
        {
            _publisher.Publish(new CharacterReachedGoldMessage(character.CharacterId, Guid));
        }

        private void OnCharacterTookGold(CharacterCollectGoldMessage message)
        {
            GoldHasBeenTaken?.Invoke();
            Dispose();
        }
    }
}