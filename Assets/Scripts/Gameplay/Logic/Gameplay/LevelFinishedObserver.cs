using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay.Logic.Gameplay
{
    public class LevelFinishedObserver : ILevelFinishedObserver
    {
        private readonly IAsyncEnumerablePublisher _publisher;
        private readonly IReadOnlyList<LevelConfig> _levelsConfig;
        private int _characterId;
        private int _levelId;

        private int _goldCollectedInSession;

        private readonly CancellationTokenSource _disposeTokenSource = new();

        public LevelFinishedObserver(IAsyncEnumerableReceiver receiver, IAsyncEnumerablePublisher publisher, IReadOnlyList<LevelConfig> levelsConfig)
        {
            _publisher = publisher;
            _levelsConfig = levelsConfig;

            receiver.Receive<LevelCreatedMessage>().Subscribe(OnLevelCreated).AddTo(_disposeTokenSource.Token);
            receiver.Receive<CharacterCollectGoldMessage>().Where(m => m.IsCharacterMatch(_characterId)).Subscribe(OnPlayerCollectGold).AddTo(_disposeTokenSource.Token);
        }

        public void Dispose()
        {
            _disposeTokenSource.Cancel();
        }

        public void BindCharacter(int id)
        {
            _characterId = id;
        }

        private void OnPlayerCollectGold(CharacterCollectGoldMessage message)
        {
            _goldCollectedInSession++;

            if (_goldCollectedInSession == _levelsConfig[_levelId].GoldHeaps)
            {
                _publisher.Publish(new LevelGoalCompletedMessage());
            }
        }

        private void OnLevelCreated(LevelCreatedMessage message)
        {
            _levelId = message.LevelId;
        }
    }
}