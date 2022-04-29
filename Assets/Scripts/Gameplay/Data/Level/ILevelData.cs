using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface ILevelData
    {
        Bounds CameraBounds { get; }
        int LevelNumber { get; }
    }
}