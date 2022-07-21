using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class BorderView : View<BorderPresenter>
    {
        [SerializeField] private BorderType _borderType;
        [SerializeField] private Transform _topPoint;
        
        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var characterView = otherCollider.TryGetCharacter();

            if (characterView != null)
            {
                _presenter.EnterBorderTrigger(characterView, _borderType, _topPoint.position.y);
            }
        }
        
        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            var characterView = otherCollider.TryGetCharacter();

            if (characterView != null)
            {
                _presenter.ExitBorderTrigger(characterView, _borderType);
            }
        }
    }
}