namespace Loderunner.Gameplay
{
    public interface IGuardiansIdPool
    {
        int GuardiansCount { get; }
        
        int GetId();
        void ReturnId(int id);
    }
}