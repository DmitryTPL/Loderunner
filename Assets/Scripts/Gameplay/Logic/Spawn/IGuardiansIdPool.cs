namespace Loderunner.Gameplay
{
    public interface IGuardiansIdPool
    {
        int GetId();
        void ReturnId(int id);
    }
}