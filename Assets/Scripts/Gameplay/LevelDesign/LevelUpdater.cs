using System.Collections.Generic;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    public class LevelUpdater : MonoBehaviour
    {
        [SerializeField] private LevelMapCreator _mapCreator;
        [SerializeField] private LevelView _levelView;
        [SerializeField] private List<PlacerBase> _placers;

        [ContextMenu("Recreate all units")]
        public void RecreateAll()
        {
            foreach (var placer in _placers)
            {
                placer.Recreate();
            }
        }

        [ContextMenu("Update map")]
        public void UpdateMap()
        {
            var map = _mapCreator.CreateMap(_placers);

            _levelView.SetPathWeightMap(map);
        }
    }
}