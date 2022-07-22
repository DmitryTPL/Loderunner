using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class RemovedWallGroundView : View<RemovedWallGroundPresenter>
    {
        [SerializeField] private BoxCollider2D _groundCollider;

        protected override void PresenterAttached()
        {
            base.PresenterAttached();
            
            _presenter.IsActive.ForEachAsync(OnActivityChanged).Forget();
        }

        private void OnActivityChanged(bool isActive)
        {
            _groundCollider.enabled = isActive;
        }
    }
}