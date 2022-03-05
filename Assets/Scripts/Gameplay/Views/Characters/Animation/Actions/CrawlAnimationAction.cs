using UnityEngine;

namespace Loderunner.Gameplay
{
    public class CrawlAnimationAction : AnimationActionBase
    {
        private readonly bool _isCrawling;

        public CrawlAnimationAction(bool isCrawling)
        {
            _isCrawling = isCrawling;
        }
        
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsCrawling, _isCrawling);
        }
    }
}