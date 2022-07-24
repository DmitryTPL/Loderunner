using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class RemovedWallView : View<RemovedWallPresenter>
    {
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private Transform _top;
        
        protected override void PresenterAttached()
        {
            base.PresenterAttached();
            
            _presenter.SetPosition(transform.position);
            _presenter.IsActive.ForEachAsync(OnActivityChanged).Forget();
        }

        private void OnTriggerStay2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();
            
            if (character != null)
            {
                _presenter.UpdateCharacterPosition(character.Position, character.CharacterId, _top.position.y);
            }
        }

        private void OnActivityChanged(bool isActive)
        {
            _collider.enabled = isActive;
        }
    }
}