using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay.Ladder
{
    public class ResetFinalLadderAnimationAction : AnimationActionBase
    {
        public override UniTask Execute(Animator animator)
        {
            animator.SetTrigger(LadderAnimationParameter.ResetFinalLadder);
            
            return UniTask.CompletedTask;
        }
    }
}