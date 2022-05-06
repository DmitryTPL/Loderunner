using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelData : ILevelData
    {
        public Bounds CameraBounds { get; set; }
        public int LevelNumber { get; set; }
        public Matrix<int> Map { get; set; }
    }
}