using UnityEngine;

namespace Loderunner.Gameplay.Ladder
{
    public class ShowFinalLadderAnimationAction : AnimationActionBase
    {
        public override void Execute(Animator animator)
        {
            animator.SetTrigger(LadderAnimationParameter.ShowFinalLadder);
        }
    }
}