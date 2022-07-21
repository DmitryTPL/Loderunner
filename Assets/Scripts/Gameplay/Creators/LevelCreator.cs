using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelCreator : ILevelCreator
    {
        private readonly Func<int, LevelView> _levelCreator;

        public LevelCreator(Func<int, LevelView> levelCreator)
        {
            _levelCreator = levelCreator;
        }

        public GameObject CreateLevel(int level, LevelConfig levelConfig)
        {
            var levelObject = _levelCreator(level);

            levelObject.SetLevelData(level, levelConfig);
            
            return levelObject.gameObject;
        }
    }
}