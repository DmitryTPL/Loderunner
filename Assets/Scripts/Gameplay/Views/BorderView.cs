using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class BorderView : View<BorderPresenter>
    {
        [SerializeField] private BorderType _borderType;
        
        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var characterView = otherCollider.gameObject.GetComponent<ICharacterInfo>();

            if (characterView != null)
            {
                _presenter.EnterBorderTrigger(characterView, _borderType);
            }
        }
        
        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            var characterView = otherCollider.gameObject.GetComponent<ICharacterInfo>();

            if (characterView != null)
            {
                _presenter.ExitBorderTrigger(characterView);
            }
        }
    }
}