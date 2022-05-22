using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IGuardianCreator
    {
        GameObject CreateGuardian(Transform spawner, int id);

        void ReturnGuardian(GameObject guardian);
    }
}