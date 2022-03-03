using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [Serializable]
    public class HorizontalBlocks
    {
        [SerializeField] private List<WallBlockView> _blocks = new();

        public List<WallBlockView> Blocks => _blocks;
    }
}