using UnityEngine;

namespace Loderunner.Gameplay
{
    public class CrawlFinishedAnimationAction : AnimationActionBase
    {
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
            animator.SetTrigger(CharacterAnimationParameter.CrawlFinished);
        }
    }
}