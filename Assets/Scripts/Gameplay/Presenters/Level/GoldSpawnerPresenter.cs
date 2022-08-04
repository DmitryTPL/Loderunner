using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GoldSpawnerPresenter : Presenter
    {
        private readonly IGoldCreator _goldCreator;
        private Transform _root;

        public GoldSpawnerPresenter(IAsyncEnumerableReceiver receiver, IGoldCreator goldCreator)
        {
            receiver.Receive<GuardianDropGoldMessage>().Subscribe(OnSpawnAboveGuardian).AddTo(DisposeCancellationToken);
            _goldCreator = goldCreator;
        }

        public void SetGoldSpawnRoot(Transform root)
        {
            _root = root;
        }

        private void OnSpawnAboveGuardian(GuardianDropGoldMessage message)
        {
            var gold = _goldCreator.CreateGold(_root);

            gold.transform.position = new Vector3(message.GuardianPosition.x, message.GuardianPosition.y + 1);
        }
    }
}