using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class FloorView : View<FloorPresenter>
    {
        [SerializeField] private Transform _fallPoint;
        
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