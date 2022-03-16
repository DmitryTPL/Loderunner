using UnityEngine;

namespace Loderunner.Gameplay
{
    public class ClimbFinishedAnimationAction : AnimationActionBase
    {
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
            animator.SetTrigger(CharacterAnimationParameter.ClimbFinished);
        }
    }
}