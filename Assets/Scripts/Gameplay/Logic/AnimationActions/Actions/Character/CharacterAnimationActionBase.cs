using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class CharacterAnimationActionBase : AnimationActionBase
    {
        protected override void ResetAll(Animator animator)
        {
            animator.SetBool(CharacterAnimationParameter.IsClimbing, false);
            animator.SetBool(CharacterAnimationParameter.IsCrawling, false);
            animator.SetBool(CharacterAnimationParameter.IsMoving, false);
            animator.SetBool(CharacterAnimationParameter.IsFalling, false);
            animator.SetFloat(CharacterAnimationParameter.MoveSpeed, 0);
        }
    }
}