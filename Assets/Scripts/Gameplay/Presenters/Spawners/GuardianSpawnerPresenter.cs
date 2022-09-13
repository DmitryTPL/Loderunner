using Cysharp.Threading.Tasks;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GuardianSpawnerPresenter : Presenter, IGuardianSpawner
    {
        private Transform _transform;
        private readonly IGuardianCreator _guardianCreator;
        
        public bool IsSpawning { get; private set; }

        public GuardianSpawnerPresenter(IGuardianCreator guardianCreator, IGuardiansSpawnCenter spawnCenter)
        {
            _guardianCreator = guardianCreator;
            spawnCenter.Register(this);

            spawnCenter.AddTo(DisposeCancellationToken);
        }

        public void SetSpawnPoint(Transform transform)
        {
            _transform = transform;
        }

        public GameObject TrySpawn(int id)
        {
            if (IsSpawning)
            {
                return null;
            }

            IsSpawning = true;
            
            var guardian = _guardianCreator.CreateGuardian(_transform, id);
            
            IsSpawning = false;

            return guardian;
        }
    }
}