using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IGameObjectCreator
    {
        GameObject CreatePlayer(Transform parent);
        GameObject CreateLevel(int level);
    }
}