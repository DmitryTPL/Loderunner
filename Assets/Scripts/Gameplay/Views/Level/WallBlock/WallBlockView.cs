using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class WallBlockView : View<WallBlockPresenter>
    {
        [SerializeField] private AnimationHandler _animationHandler;
        [SerializeField] private RemovedWallView _removedWallView;

        private void Awake()
        {
            _presenter.SetPosition(transform.position);
            _presenter.SetRemovedWallPresenter(_removedWallView.Presenter);
            _presenter.CurrentWallBlockLifeState.ForEachAsync(OnWallBlockRemoveStateChanged, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private void OnWallBlockRemoveStateChanged(WallBlockLifeState state)
        {
            switch (state)
            {
                case WallBlockLifeState.Removing:
                    _animationHandler.ApplyAnimation(new RemoveBlockAnimationAction(true));
                    break;
                case WallBlockLifeState.Restoring:
                    _animationHandler.ApplyAnimation(new RemoveBlockAnimationAction(false));
                    _animationHandler.ApplyAnimation(new RestoreBlockAnimationAction(true));
                    break;
                case WallBlockLifeState.Restored:
                    _animationHandler.ApplyAnimation(new RestoreBlockAnimationAction(false));
                    break;
            }
        }
    }
}