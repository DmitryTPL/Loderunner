using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class BorderView : View<BorderPresenter>
    {
        [SerializeField] private BorderType _borderType;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var characterView = collider.gameObject.GetComponent<ICharacterView>();

            if (characterView != null)
            {
                _presenter.EnterBorderTrigger(characterView, _borderType);
            }
        }
        
        private void OnTriggerExit2D(Collider2D collider)
        {
            var characterView = collider.gameObject.GetComponent<ICharacterView>();

            if (characterView != null)
            {
                _presenter.ExitBorderTrigger(characterView, _borderType);
            }
        }
    }
}