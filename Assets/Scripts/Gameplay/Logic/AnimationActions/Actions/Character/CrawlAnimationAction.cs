using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class CrawlAnimationAction : AnimationActionWithMoveSpeed
    {
        public CrawlAnimationAction(float moveSpeed) : base(moveSpeed)
        {
        }
        
        public override async UniTask Execute(Animator animator)
        {
            ResetAll(animator);
            
            await base.Execute(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsCrawling, true);
        }
    }
}