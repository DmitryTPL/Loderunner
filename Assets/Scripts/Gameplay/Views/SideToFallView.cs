using System;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SideToFallView : View<SideToFallPresenter>
    {
        [SerializeField] private SideToFallType _sideToFall;
        [SerializeField] private BoxCollider2D _collider;

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var characterView = otherCollider.gameObject.GetComponent<ICharacterInfo>();

            if (characterView != null)
            {
                _presenter.ReachingSideToFall(characterView, GetFallPoint(), _sideToFall);
            }
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            var characterView = otherCollider.gameObject.GetComponent<ICharacterInfo>();

            if (characterView != null)
            {
                _presenter.MoveAwayFromSideToFall(characterView, _sideToFall);
            }
        }

        private float GetFallPoint()
        {
            switch (_sideToFall)
            {
                case SideToFallType.Left:
                    return _collider.transform.position.x - _presenter.GameConfig.CellSize / 2;
                case SideToFallType.Right:
                    return _collider.transform.position.x + _collider.offset.x + _collider.size.x / 2 + _presenter.GameConfig.CellSize / 2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}