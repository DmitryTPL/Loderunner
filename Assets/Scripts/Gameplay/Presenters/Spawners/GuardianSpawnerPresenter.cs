using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GuardianSpawnerPresenter : Presenter
    {
        public event Action NeedToSpawnGuardian;

        private readonly IGuardianCreator _guardianCreator;
        private readonly IGuardiansIdPool _guardiansIdPool;

        public GuardianSpawnerPresenter(IAsyncEnumerableReceiver receiver, IGuardianCreator guardianCreator, IGuardiansIdPool guardiansIdPool)
        {
            _guardianCreator = guardianCreator;
            _guardiansIdPool = guardiansIdPool;
            
            receiver.Receive<LevelCreatedMessage>().Subscribe(OnLevelCreated).AddTo(DisposeCancellationToken);
        }

        public void Spawn(Transform transform)
        {
            _guardianCreator.CreateGuardian(transform, _guardiansIdPool.GetId());
        }

        private void OnLevelCreated(LevelCreatedMessage message)
        {
            NeedToSpawnGuardian?.Invoke();
        }
    }
}