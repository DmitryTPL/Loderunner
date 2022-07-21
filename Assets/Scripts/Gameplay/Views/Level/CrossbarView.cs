using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class CrossbarView : View<CrossbarPresenter>
    {
        [SerializeField] private Transform _left;
        [SerializeField] private Transform _right;

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();
            
            if (character != null)
            {
                _presenter.CharacterEnterCrossbar(character, _left.position.x, _right.position.x, _left.position.y);
            }
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();
            
            if (character != null)
            {
                _presenter.PlayerExitCrossbar(character);
            }
        }
    }
}