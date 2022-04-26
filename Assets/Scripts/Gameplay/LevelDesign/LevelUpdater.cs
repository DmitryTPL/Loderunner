using System.Collections.Generic;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    public class LevelUpdater : MonoBehaviour
    {
        [SerializeField] private List<PlacerBase> _placers;
        
        [ContextMenu("Recreate all units")]
        public void RecreateAll()
        {
            foreach (var placer in _placers)
            {
                placer.Recreate();
            }
        }
    }
}