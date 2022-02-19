using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [Serializable]
    public class HorizontalBlocks
    {
        [SerializeField] private List<GameObject> _blocks = new List<GameObject>();

        public List<GameObject> Blocks => _blocks;
    }
}