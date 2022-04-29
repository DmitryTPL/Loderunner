using UnityEngine;

namespace Loderunner.Gameplay.Ladder
{
    public class ResetFinalLadderAnimationAction : AnimationActionBase
    {
        public override void Execute(Animator animator)
        {
            animator.SetTrigger(LadderAnimationParameter.ResetFinalLadder);
        }
    }
}