using UnityEngine;

namespace Loderunner.Gameplay
{
    public sealed class CharacterRemoveBlockAnimationAction: CharacterAnimationActionBase
    {
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsRemovingBlock, true);
        }
    }
}