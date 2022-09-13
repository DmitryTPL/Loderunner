using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class MoveAnimationAction : AnimationActionWithMoveSpeed
    {
        public MoveAnimationAction(float moveSpeed) : base(moveSpeed)
        {
        }
        
        public override UniTask Execute(Animator animator)
        {
            ResetAll(animator);
            
            base.Execute(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsMoving, true);
            
            return UniTask.CompletedTask;
        }
    }
}