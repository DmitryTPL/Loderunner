using System.Collections.Generic;
using Loderunner.Service;

namespace Loderunner.Gameplay
{
    public class PrepareScenePresenter : Presenter
    {
        private readonly ILevelCreator _levelCreator;
        private readonly IReadOnlyList<LevelConfig> _levelsConfig;

        public PrepareScenePresenter(ILevelCreator levelCreator, IReadOnlyList<LevelConfig> levelsConfig)
        {
            _levelCreator = levelCreator;
            _levelsConfig = levelsConfig;
        }

        public void CreateLevel(int levelIndex)
        {
            _levelCreator.CreateLevel(levelIndex, _levelsConfig[levelIndex]);
        }
    }
}