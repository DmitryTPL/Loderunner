using UnityEngine;

namespace Loderunner.Gameplay
{
    public sealed class CharacterRemoveBlockFinishedAnimationAction: CharacterAnimationActionBase
    {
        public override void Execute(Animator animator)
        {
            animator.SetBool(CharacterAnimationParameter.IsRemovingBlock, false);
            ResetAll(animator);
        }
    }
}