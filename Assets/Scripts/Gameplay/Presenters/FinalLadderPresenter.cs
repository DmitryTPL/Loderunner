using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class FinalLadderPresenter : Presenter
    {
        public event Action LevelCompleted;
        public event Action LevelReset;
        
        public FinalLadderPresenter(IAsyncEnumerableReceiver receiver)
        {
            receiver.Receive<LevelGoalCompletedMessage>().Subscribe(OnLevelCompleted).AddTo(DisposeCancellationToken);
        }

        private void OnLevelCompleted(LevelGoalCompletedMessage message)
        {
            LevelCompleted?.Invoke();
        }
    }
}