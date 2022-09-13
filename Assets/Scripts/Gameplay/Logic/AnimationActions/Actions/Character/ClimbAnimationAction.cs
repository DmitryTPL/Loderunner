using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class ClimbAnimationAction : AnimationActionWithMoveSpeed
    {
        public ClimbAnimationAction(float moveSpeed) : base(moveSpeed)
        {
        }
        
        public override async UniTask Execute(Animator animator)
        {
            ResetAll(animator);
            
            await base.Execute(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsClimbing, true);
        }
    }
}