using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class RemovableWallBlockView : WallBlock<RemovableWallBlockPresenter>
    {
        [SerializeField] private AnimationHandler _animationHandler;
        [SerializeField] private RemovedWallView _removedWallView;
        [SerializeField] private RemovedWallGroundView _removedWallGroundView;

        protected override void PresenterAttached()
        {
            base.PresenterAttached();
            
            _presenter.CurrentWallBlockLifeState.ForEachAsync(OnWallBlockRemoveStateChanged, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private void Start()
        {
            _presenter.SetRemovedWallPresenter(_removedWallView.Presenter);
            _presenter.SetRemovedWallGroundPresenter(_removedWallGroundView.Presenter);
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