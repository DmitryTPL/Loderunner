using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelView : View<LevelPresenter>
    {
        [SerializeField] private BoxCollider2D _boundsCollider;
        [SerializeField] private Matrix<int> _pathWeightMap;

        private int _levelNumber;
        private LevelConfig _levelConfig;
        
        private void Start()
        {
            _presenter.SetCameraBounds(_boundsCollider.bounds);
            
            _presenter.LevelCreated(_levelNumber, _pathWeightMap, _levelConfig);
        }

        public void SetLevelData(int levelNumber, LevelConfig levelConfig)
        {
            _levelNumber = levelNumber;
            _levelConfig = levelConfig;
        }

        public void SetPathWeightMap(Matrix<int> pathWeightMap)
        {
            _pathWeightMap = pathWeightMap;
        }
    }
}