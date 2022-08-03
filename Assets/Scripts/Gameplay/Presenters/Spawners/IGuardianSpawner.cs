using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IGuardianSpawner
    {
        public GameObject TrySpawn(int id);
        bool IsSpawning { get; }
    }
}