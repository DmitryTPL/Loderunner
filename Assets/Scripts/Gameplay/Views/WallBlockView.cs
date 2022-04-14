using System;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class WallBlockView : View<WallBlockPresenter>
    {
        [SerializeField] private AnimationHandler _animationHandler;

        private void Awake()
        {
            _presenter.SetPosition(transform.position);
        }

        private void Start()
        {
            _presenter.BlockRemoveBegan += OnRemoveBegan;
            _presenter.BlockRestoreBegan += OnRestoreBegan;
            _presenter.BlockRestoreCompleted += OnRestoreCompleted;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _presenter.BlockRemoveBegan -= OnRemoveBegan;
            _presenter.BlockRestoreBegan -= OnRestoreBegan;
            _presenter.BlockRestoreCompleted -= OnRestoreCompleted;
        }

        private void OnRemoveBegan()
        {
            _animationHandler.ApplyAnimation(new RemoveBlockAnimationAction(true));
        }

        private void OnRestoreBegan()
        {
            _animationHandler.ApplyAnimation(new RemoveBlockAnimationAction(false));
            _animationHandler.ApplyAnimation(new RestoreBlockAnimationAction(true));
        }

        private void OnRestoreCompleted()
        {
            _animationHandler.ApplyAnimation(new RestoreBlockAnimationAction(false));
        }
    }
}