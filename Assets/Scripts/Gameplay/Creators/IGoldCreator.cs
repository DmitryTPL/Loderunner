using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IGoldCreator
    {
        GameObject CreateGold(Transform parent);
    }
}