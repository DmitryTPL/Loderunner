using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class PlayerCreator : IPlayerCreator
    {
        private readonly Func<Transform, PlayerView> _playerCreator;

        public PlayerCreator(Func<Transform, PlayerView> playerCreator)
        {
            _playerCreator = playerCreator;
        }

        public GameObject CreatePlayer(Transform parent)
        {
            return _playerCreator(parent).gameObject;
        }
    }
}