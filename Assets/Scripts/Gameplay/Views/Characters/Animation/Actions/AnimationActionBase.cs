using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class AnimationActionBase
    {
        public abstract void Execute(Animator animator);

        protected void ResetAll(Animator animator)
        {
            animator.SetBool(CharacterAnimationParameter.IsClimbing, false);
            animator.SetBool(CharacterAnimationParameter.IsCrawling, false);
            animator.SetBool(CharacterAnimationParameter.IsMoving, false);
            animator.SetBool(CharacterAnimationParameter.IsFalling, false);
        }
    }
}