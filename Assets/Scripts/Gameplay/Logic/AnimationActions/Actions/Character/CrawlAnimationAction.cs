using UnityEngine;

namespace Loderunner.Gameplay
{
    public class CrawlAnimationAction : AnimationActionWithMoveSpeed
    {
        public CrawlAnimationAction(float moveSpeed) : base(moveSpeed)
        {
        }
        
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
            
            base.Execute(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsCrawling, true);
        }
    }
}