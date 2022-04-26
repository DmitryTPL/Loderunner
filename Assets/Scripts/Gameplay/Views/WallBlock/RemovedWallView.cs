using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class RemovedWallView : View<RemovedWallPresenter>
    {
        [SerializeField] private BoxCollider2D _collider;
        
        private void Awake()
        {
            _presenter.SetPosition(transform.position);
            _presenter.IsActive.ForEachAsync(OnActivityChanged).Forget();
        }

        private void OnTriggerStay2D(Collider2D otherCollider)
        {
            var characterView = otherCollider.gameObject.GetComponent<ICharacterInfo>();
            
            if (characterView != null)
            {
                _presenter.UpdateCharacterPosition(characterView.Position, characterView.CharacterId);
            }
        }

        private void OnActivityChanged(bool isActive)
        {
            _collider.enabled = isActive;
        }
    }
}