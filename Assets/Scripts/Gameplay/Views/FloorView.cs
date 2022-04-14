using System;
using System.Collections.Generic;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class FloorView : View<FloorPresenter>
    {
        [SerializeField] private Transform _fallPoint;
        [SerializeField] private List<WallBlockView> _wallBlocks;

        public List<WallBlockView> WallBlocks
        {
            get => _wallBlocks;
            set => _wallBlocks = value;
        }

        private void Awake()
        {
            _presenter.SetFloorStartPosition(transform.position);
        }

        private void Start()
        {
            foreach (var wallBlock in _wallBlocks)
            {
                _presenter.AddWallBlockPresenter(wallBlock.Presenter);
            }
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var characterView = otherCollider.gameObject.GetComponent<ICharacterInfo>();

            if (characterView != null)
            {
                _presenter.FloorReached(characterView, gameObject.GetInstanceID(), _fallPoint.position.y);
            }
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            var characterView = otherCollider.gameObject.GetComponent<ICharacterInfo>();

            if (characterView != null)
            {
                _presenter.GotOffTheFloor(characterView, gameObject.GetInstanceID());
            }
        }
    }
}