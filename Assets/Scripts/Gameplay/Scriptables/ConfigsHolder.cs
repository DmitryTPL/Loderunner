﻿using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [CreateAssetMenu(fileName = "Gameplay Configs", menuName = "ScriptableObjects/Gameplay Configs", order = 1)]
    public class ConfigsHolder : ScriptableObject
    {
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private GuardianConfig _guardianConfig;
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private WallBlockRemoveConfig _wallBlockRemoveConfig;

        public PlayerConfig PlayerConfig => _playerConfig;
        public GuardianConfig GuardianConfig => _guardianConfig;
        public GameConfig GameConfig => _gameConfig;
        public WallBlockRemoveConfig WallBlockRemoveConfig => _wallBlockRemoveConfig;
    }

    public interface ICharacterConfig
    {
        public float WalkSpeed { get; }
        public float ClimbSpeed { get; }
        public float CrawlSpeed { get; }
        public float FallSpeed { get; }
    }

    [Serializable]
    public class PlayerConfig : ICharacterConfig
    {
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _climbSpeed;
        [SerializeField] private float _crawlSpeed;
        [SerializeField] private float _fallSpeed;
        [SerializeField] private float _dieTimeout;

        public float WalkSpeed => _walkSpeed;
        public float ClimbSpeed => _climbSpeed;
        public float CrawlSpeed => _crawlSpeed;
        public float FallSpeed => _fallSpeed;
    }

    [Serializable]
    public class GuardianConfig : ICharacterConfig
    {
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _climbSpeed;
        [SerializeField] private float _crawlSpeed;
        [SerializeField] private float _fallSpeed;
        [SerializeField] private float _stuckInRemovedBlockTimeout;
        [SerializeField] private float _respawnTimeout;

        public float WalkSpeed => _walkSpeed;
        public float ClimbSpeed => _climbSpeed;
        public float CrawlSpeed => _crawlSpeed;
        public float FallSpeed => _fallSpeed;
        public float StuckInRemovedBlockTimeout => _stuckInRemovedBlockTimeout;
        public float RespawnTimeout => _respawnTimeout;
    }

    [Serializable]
    public class GameConfig
    {
        [SerializeField] private float _cellSize;
        [SerializeField] private float _movementThreshold;
        [SerializeField] private float _smoothPlayerCameraMovementRatio;

        public float CellSize => _cellSize;
        public float MovementThreshold => _movementThreshold;
        public float SmoothPlayerCameraMovementRatio => _smoothPlayerCameraMovementRatio;
    }
    
    [Serializable]
    public class WallBlockRemoveConfig
    {
        [SerializeField] private float _removeTime;
        [SerializeField] private float _removedStateTime;
        [SerializeField] private float _restoreTime;

        public float RemoveTime => _removeTime;
        public float RemovedStateTime => _removedStateTime;
        public float RestoreTime => _restoreTime;
    }
}