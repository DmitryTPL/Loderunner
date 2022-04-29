using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [CreateAssetMenu(fileName = "LevelsConfigs", menuName = "ScriptableObjects/Levels Configs", order = 2)]
    public class LevelsConfigsHolder : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> _levels;

        public IReadOnlyList<LevelConfig> Levels => _levels;
    }

    [Serializable]
    public class LevelConfig
    {
        [SerializeField] private int _goldHeaps;
        [SerializeField] private int _guards;

        public int GoldHeaps => _goldHeaps;
        public int Guards => _guards;
    }
}