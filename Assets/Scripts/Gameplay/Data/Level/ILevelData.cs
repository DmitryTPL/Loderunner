using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface ILevelData
    {
        Bounds CameraBounds { get; }
        int LevelNumber { get; }
        Matrix<int> Map { get; }
        LevelConfig Config { get; }
    }
}