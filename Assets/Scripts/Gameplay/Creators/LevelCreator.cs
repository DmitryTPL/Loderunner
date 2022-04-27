using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelCreator : ILevelCreator
    {
        private readonly Func<int, LevelView> _levelCreator;

        public LevelCreator(Func<Transform, PlayerView> playerCreator, Func<int, LevelView> levelCreator)
        {
            _levelCreator = levelCreator;
        }

        public GameObject CreateLevel(int level)
        {
            return _levelCreator(level).gameObject;
        }
    }
}