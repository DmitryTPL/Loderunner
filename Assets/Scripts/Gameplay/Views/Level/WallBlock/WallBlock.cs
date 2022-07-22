using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class WallBlock<T> : View<T>
        where T : WallBlockPresenter
    {
        [SerializeField] private BoxCollider2D _leftBorder;
        [SerializeField] private BoxCollider2D _rightBorder;

        protected override void PresenterAttached()
        {
            base.PresenterAttached();
            
            _presenter.SetPosition(transform.position);

            _presenter.IsLeftBorderActive.ForEachAsync(OnLeftBorderActivityChanged).Forget();
            _presenter.IsRightBorderActive.ForEachAsync(OnRightBorderActivityChanged).Forget();
        }

        private void OnLeftBorderActivityChanged(bool isActive)
        {
            _leftBorder.enabled = isActive;
        }

        private void OnRightBorderActivityChanged(bool isActive)
        {
            _rightBorder.enabled = isActive;
        }
    }
}