using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelView : View<LevelPresenter>
    {
        [SerializeField] private BoxCollider2D _boundsCollider;
        [SerializeField] private Matrix<int> _pathWeightMap;

        private int _levelNumber;
        
        private void Start()
        {
            _presenter.SetCameraBounds(_boundsCollider.bounds);
            
            _presenter.LevelCreated(_levelNumber, _pathWeightMap);
        }

        public void SetLevelNumber(int levelNumber)
        {
            _levelNumber = levelNumber;
        }

        public void SetPathWeightMap(Matrix<int> pathWeightMap)
        {
            _pathWeightMap = pathWeightMap;
        }
    }
}