using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IPlayerCreator
    {
        GameObject CreatePlayer(Transform parent);
    }
}