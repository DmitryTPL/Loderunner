using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GameObjectCreator : IGameObjectCreator
    {
        private readonly Func<Transform, PlayerView> _playerCreator;
        private readonly Func<int, LevelView> _levelCreator;

        public GameObjectCreator(Func<Transform, PlayerView> playerCreator, Func<int, LevelView> levelCreator)
        {
            _playerCreator = playerCreator;
            _levelCreator = levelCreator;
        }
        
        public GameObject CreatePlayer(Transform parent)
        {
             var gameobject = _playerCreator(parent).gameObject;

             return gameobject;
        }

        public GameObject CreateLevel(int level)
        {
            return _levelCreator(level).gameObject;
        }
    }
}