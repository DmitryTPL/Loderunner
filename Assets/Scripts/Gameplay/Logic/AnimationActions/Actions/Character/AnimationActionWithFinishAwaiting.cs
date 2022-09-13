using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class AnimationActionWithFinishAwaiting : CharacterAnimationActionBase
    {
        private bool _animationFinished;
        
        protected abstract int Trigger { get; }
        
        public override async UniTask Execute(Animator animator)
        {
            var stateBehaviour = animator.GetBehaviour<AnimationFinishedStateBehaviour>();

            stateBehaviour.Finished += OnAnimationFinished;
            
            ResetAll(animator);
            
            animator.SetTrigger(Trigger);

            await UniTask.WaitUntil(() => _animationFinished);
            
            stateBehaviour.Finished -= OnAnimationFinished;
        }

        private void OnAnimationFinished()
        {
            _animationFinished = true;
        }
    }
}