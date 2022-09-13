using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class CrawlFinishedAnimationAction : CharacterAnimationActionBase
    {
        public override UniTask Execute(Animator animator)
        {
            ResetAll(animator);
            animator.SetTrigger(CharacterAnimationParameter.CrawlFinished);
            
            return UniTask.CompletedTask;
        }
    }
}