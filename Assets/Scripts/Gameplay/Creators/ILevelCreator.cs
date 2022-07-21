using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface ILevelCreator
    {
        GameObject CreateLevel(int level, LevelConfig levelConfig);
    }
}