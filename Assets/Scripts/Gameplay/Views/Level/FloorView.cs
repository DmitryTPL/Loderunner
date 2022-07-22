using System.Collections.Generic;
using Loderunner.Service;
using UnityEngine;
using UnityEngine.Serialization;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class FloorView : View<FloorPresenter>
    {
        [SerializeField] private Transform _fallPoint;
        [FormerlySerializedAs("_wallBlocks")] [SerializeField] private List<RemovableWallBlockView> _removableWallBlocks;
        [SerializeField] private List<PermanentWallBlockView> _permanentWallBlocks;

        public List<RemovableWallBlockView> WallBlocks
        {
            get => _removableWallBlocks;
            set => _removableWallBlocks = value;
        }

        private void Awake()
        {
            _presenter.SetFloorStartPosition(transform.position);
        }

        private void Start()
        {
            foreach (var wallBlock in _removableWallBlocks)
            {
                _presenter.AddWallBlock(wallBlock.Presenter, wallBlock.transform.GetSiblingIndex());
            }
            
            foreach (var wallBlock in _permanentWallBlocks)
            {
                _presenter.AddWallBlock(wallBlock.Presenter, wallBlock.transform.GetSiblingIndex());
            }
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();

            if (character != null)
            {
                _presenter.FloorReached(character, _fallPoint.position.y);
            }
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();

            if (character != null)
            {
                _presenter.GotOffTheFloor(character);
            }
        }
    }
}