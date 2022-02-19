using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/Game config", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private PlayerConfig _playerConfig;

        public PlayerConfig PlayerConfig => _playerConfig;
    }

    [Serializable]
    public class PlayerConfig
    {
        [SerializeField] private Vector2 _speed;

        public Vector2 Speed => _speed;
    }
}