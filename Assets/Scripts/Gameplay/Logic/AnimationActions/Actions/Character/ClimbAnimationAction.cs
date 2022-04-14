using UnityEngine;

namespace Loderunner.Gameplay
{
    public class ClimbAnimationAction : AnimationActionWithMoveSpeed
    {
        public ClimbAnimationAction(float moveSpeed) : base(moveSpeed)
        {
        }
        
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
            
            base.Execute(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsClimbing, true);
        }
    }
}