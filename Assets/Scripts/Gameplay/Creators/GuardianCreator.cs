﻿using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GuardianCreator : PooledCreator<GuardianView, GuardianPresenter>, IGuardianCreator
    {
        private readonly struct InstantiateData : IInstantiateData
        {
            public Transform Parent { get; }
            public Vector3 LocalPosition { get; }

            public InstantiateData(Transform parent, Vector3 localPosition)
            {
                Parent = parent;
                LocalPosition = localPosition;
            }
        }

        private readonly Func<Transform, GuardianView> _guardianFactory;

        public GuardianCreator(Transform pool, Func<Transform, GuardianView> guardianFactory) : base(pool)
        {
            _guardianFactory = guardianFactory;
        }

        protected override GuardianView Instantiate(IInstantiateData rawData)
        {
            var data = (InstantiateData)rawData;

            return _guardianFactory(data.Parent);
        }

        public GameObject CreateGuardian(Transform spawner, int id)
        {
            var guardian = Pop(new InstantiateData(spawner, Vector3.zero));

            guardian.CharacterId = id;
            
            return guardian.gameObject;
        }

        public void ReturnGuardian(GameObject guardian)
        {
            if (guardian.TryGetComponent<GuardianView>(out var guardianComponent))
            {
                Push(guardianComponent);
            }
        }
    }
}