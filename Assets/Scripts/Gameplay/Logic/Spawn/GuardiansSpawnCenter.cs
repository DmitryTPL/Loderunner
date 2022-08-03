using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;
using Random = System.Random;

namespace Loderunner.Gameplay
{
    public class GuardiansSpawnCenter : IDisposable, IGuardiansSpawnCenter
    {
        private readonly CancellationTokenSource _disposeCancellationTokenSource = new();
        private CancellationToken DisposeCancellationToken => _disposeCancellationTokenSource.Token;

        private readonly IAsyncEnumerablePublisher _publisher;
        private readonly ILevelData _levelData;
        private readonly IGuardianCreator _guardianCreator;

        private readonly List<IGuardianSpawner> _spawners = new();
        private readonly List<GameObject> _guardians = new();

        public GuardiansSpawnCenter(IAsyncEnumerableReceiver receiver, IAsyncEnumerablePublisher publisher, ILevelData levelData, 
            IGuardianCreator guardianCreator)
        {
            _publisher = publisher;
            _levelData = levelData;
            _guardianCreator = guardianCreator;

            receiver.Receive<LevelCreatedMessage>().Subscribe(OnLevelCreated).AddTo(DisposeCancellationToken);
            receiver.Receive<RespawnGuardianMessage>().Subscribe(OnRespawnGuardian).AddTo(DisposeCancellationToken);
        }

        public void Register(IGuardianSpawner spawner)
        {
            _spawners.Add(spawner);
        }

        public void Dispose()
        {
            _spawners.Clear();
            _disposeCancellationTokenSource?.Dispose();
        }

        private void OnLevelCreated(LevelCreatedMessage message)
        {
            var spawned = 0;

            foreach (var spawner in _spawners)
            {
                if (spawned >= _levelData.Config.Guardians)
                {
                    break;
                }

                var guardian = spawner.TrySpawn(spawned);

                if (guardian == null)
                {
                    this.LogError("Error while creating a guardian while level creation.");
                    
                    break;
                }

                _guardians.Add(guardian);

                spawned++;
            }
        }

        private void OnRespawnGuardian(RespawnGuardianMessage message)
        {
            _guardianCreator.ReturnGuardian(_guardians[message.CharacterId]);
            
            var random = new Random();

            for (;;)
            {
                var spawnerIndex = random.Next(_spawners.Count);

                if (_spawners[spawnerIndex].IsSpawning)
                {
                    continue;
                }
                
                if (_spawners[spawnerIndex].TrySpawn(message.CharacterId))
                {
                    _publisher.Publish(new GuardianRespawnedMessage(message.CharacterId));
                    
                    break;
                }
            }
        }
    }
}