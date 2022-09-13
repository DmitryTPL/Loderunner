using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay.Ladder
{
    public class ShowFinalLadderAnimationAction : AnimationActionBase
    {
        public override UniTask Execute(Animator animator)
        {
            animator.SetTrigger(LadderAnimationParameter.ShowFinalLadder);
            
            return UniTask.CompletedTask;
        }
    }
}