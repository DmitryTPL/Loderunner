using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelPresenter: Presenter
    {
        private readonly LevelData _levelData;

        public LevelPresenter(LevelData levelData)
        {
            _levelData = levelData;
        }

        public void SetCameraBounds(Bounds bounds)
        {
            _levelData.CameraBounds = bounds;
        }
    }
}