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
        }

        public void SetSpawnPoint(Transform transform)
        {
            _transform = transform;
        }

        public bool TrySpawn(int id)
        {
            if (IsSpawning)
            {
                return false;
            }

            IsSpawning = true;
            
            _guardianCreator.CreateGuardian(_transform, id);
            
            IsSpawning = false;

            return true;
        }
    }
}