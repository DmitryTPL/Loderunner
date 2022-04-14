using System.Collections.Generic;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    public class LevelUpdater : MonoBehaviour
    {
        [SerializeField] private List<WallPlacer> _wallPlacers;
        
        [ContextMenu("Recreate all walls")]
        public void RecreateAllWalls()
        {
            foreach (var wallPlacer in _wallPlacers)
            {
                wallPlacer.RecreateBlocks();
            }
        }
    }
}