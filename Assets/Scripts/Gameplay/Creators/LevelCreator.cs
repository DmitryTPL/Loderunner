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

        public GameObject CreateLevel(int level)
        {
            var levelObject = _levelCreator(level);

            levelObject.SetLevelNumber(level);
            
            return levelObject.gameObject;
        }
    }
}