using UnityEngine;

namespace Loderunner.Gameplay
{
    public class MoveAnimationAction : AnimationActionWithMoveSpeed
    {
        public MoveAnimationAction(float moveSpeed) : base(moveSpeed)
        {
        }
        
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
            
            base.Execute(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsMoving, true);
        }
    }
}