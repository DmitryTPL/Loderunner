namespace Loderunner.Gameplay
{
    public interface IGuardianSpawner
    {
        public bool TrySpawn(int id);
        bool IsSpawning { get; }
    }
}