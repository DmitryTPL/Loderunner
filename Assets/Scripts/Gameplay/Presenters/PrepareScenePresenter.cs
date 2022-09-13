using System.Collections.Generic;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class PrepareScenePresenter : Presenter
    {
        private readonly ILevelCreator _levelCreator;
        private readonly IReadOnlyList<LevelConfig> _levelsConfig;

        private GameObject _currentLevelObject;
        private int _currentLevelIndex;

        public PrepareScenePresenter(ILevelCreator levelCreator, IReadOnlyList<LevelConfig> levelsConfig, IAsyncEnumerableReceiver receiver)
        {
            _levelCreator = levelCreator;
            _levelsConfig = levelsConfig;

            receiver.Receive<LevelResetMessage>().Subscribe(OnLevelReset);
        }

        public void CreateLevel(int levelIndex)
        {
            _currentLevelIndex = levelIndex;
            _currentLevelObject = _levelCreator.CreateLevel(levelIndex, _levelsConfig[levelIndex]);
        }

        private void OnLevelReset(LevelResetMessage _)
        {
            Object.Destroy(_currentLevelObject);
            
            CreateLevel(_currentLevelIndex);
        }
    }
}