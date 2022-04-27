using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GoldCreator : IGoldCreator
    {
        private readonly Func<Transform, GoldView> _goldCreator;

        public GoldCreator(Func<Transform, GoldView> goldCreator)
        {
            _goldCreator = goldCreator;
        }
        
        public GameObject CreateGold(Transform parent)
        {
            return _goldCreator(parent).gameObject;
        }
    }
}