using UnityEngine;

namespace Loderunner.Gameplay
{
    public class FallingAnimationAction : CharacterAnimationActionBase
    {
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsFalling, true);
        }
    }
}