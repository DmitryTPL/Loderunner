using UnityEngine;

namespace Loderunner.Gameplay
{
    public class FallingAnimationAction : AnimationActionBase
    {
        public override void Execute(Animator animator)
        {
            animator.SetBool(CharacterAnimationParameter.IsClimbing, false);
            animator.SetBool(CharacterAnimationParameter.IsClimbing, false);
            animator.SetBool(CharacterAnimationParameter.IsMoving, false);
            animator.SetBool(CharacterAnimationParameter.IsFalling, true);
        }
    }
}