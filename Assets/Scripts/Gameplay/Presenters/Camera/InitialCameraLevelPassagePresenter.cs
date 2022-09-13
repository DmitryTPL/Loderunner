using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class InitialCameraLevelPassagePresenter : Presenter
    {
        private readonly IAsyncEnumerablePublisher _publisher;

        public event Action LevelCreated;

        public InitialCameraLevelPassagePresenter(IAsyncEnumerablePublisher publisher, IAsyncEnumerableReceiver receiver)
        {
            _publisher = publisher;

            receiver.Receive<LevelCreatedMessage>().Subscribe(OnLevelCreated).AddTo(DisposeCancellationToken);
        }

        private void OnLevelCreated(LevelCreatedMessage obj)
        {
            LevelCreated?.Invoke();
        }

        public void PassageCompleted()
        {
            _publisher.Publish(new GameStartedMessage());
        }
    }
}