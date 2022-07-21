using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class GuardiansSpawnCenter : IDisposable, IGuardiansSpawnCenter
    {
        private readonly CancellationTokenSource _disposeCancellationTokenSource = new();
        private CancellationToken DisposeCancellationToken => _disposeCancellationTokenSource.Token;

        private readonly IGuardiansIdPool _guardiansIdPool;
        private readonly ILevelData _levelData;

        private readonly List<IGuardianSpawner> _spawners = new();

        public GuardiansSpawnCenter(IAsyncEnumerableReceiver receiver, IGuardiansIdPool guardiansIdPool, ILevelData levelData)
        {
            _guardiansIdPool = guardiansIdPool;
            _levelData = levelData;

            receiver.Receive<LevelCreatedMessage>().Subscribe(OnLevelCreated).AddTo(DisposeCancellationToken);
        }

        public void Register(IGuardianSpawner spawner)
        {
            _spawners.Add(spawner);
        }

        private void OnLevelCreated(LevelCreatedMessage obj)
        {
            var spawned = 0;

            foreach (var spawner in _spawners)
            {
                if (spawned >= _levelData.Config.Guardians)
                {
                    break;
                }

                spawner.TrySpawn(_guardiansIdPool.GetId());

                spawned++;
            }
        }

        public void Dispose()
        {
            _spawners.Clear();
            _disposeCancellationTokenSource?.Dispose();
        }
    }
}