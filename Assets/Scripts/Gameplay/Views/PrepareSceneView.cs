using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class PrepareSceneView : View<PrepareScenePresenter>
    {
        [SerializeField] private Transform _levelPosition;
        
        private void Start()
        {
            InitLevel(1);
        }

        private void InitLevel(int levelIndex)
        {
            var level = _presenter.CreateLevel(_levelPosition, levelIndex);
            _presenter.CreatePlayer(level.PlayerStartPosition);
        }
    }
}