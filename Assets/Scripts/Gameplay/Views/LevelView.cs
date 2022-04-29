using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelView : View<LevelPresenter>
    {
        [SerializeField] private BoxCollider2D _boundsCollider;

        private int _levelNumber;
        
        private void Start()
        {
            _presenter.SetCameraBounds(_boundsCollider.bounds);
            
            _presenter.LevelCreated(_levelNumber);
        }

        public void SetLevelNumber(int levelNumber)
        {
            _levelNumber = levelNumber;
        }
    }
}