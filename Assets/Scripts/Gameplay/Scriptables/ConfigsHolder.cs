using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [CreateAssetMenu(fileName = "ConfigsHolder", menuName = "ScriptableObjects/Configs holder", order = 1)]
    public class ConfigsHolder : ScriptableObject
    {
        [SerializeField] private CharacterConfig _playerConfig;
        [SerializeField] private GameConfig _gameConfig;

        public CharacterConfig PlayerConfig => _playerConfig;
        public GameConfig GameConfig => _gameConfig;
    }

    public interface ICharacterConfig
    {
        public float WalkSpeed { get; }
        public float ClimbSpeed { get; }
        public float CrawlSpeed { get; }
        public float FallSpeed { get; }
    }

    [Serializable]
    public class CharacterConfig : ICharacterConfig
    {
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _climbSpeed;
        [SerializeField] private float _crawlSpeed;
        [SerializeField] private float _fallSpeed;

        public float WalkSpeed => _walkSpeed;
        public float ClimbSpeed => _climbSpeed;
        public float CrawlSpeed => _crawlSpeed;
        public float FallSpeed => _fallSpeed;
    }

    [Serializable]
    public class GameConfig
    {
        [SerializeField] private float _cellSize;
        [SerializeField] private float _movementThreshold;

        public float CellSize => _cellSize;
        public float MovementThreshold => _movementThreshold;
    }
}