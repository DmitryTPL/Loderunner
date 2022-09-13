using System;

namespace Loderunner.Gameplay
{
    public interface IGuardiansSpawnCenter: IDisposable
    {
        void Register(IGuardianSpawner spawner);
    }
}