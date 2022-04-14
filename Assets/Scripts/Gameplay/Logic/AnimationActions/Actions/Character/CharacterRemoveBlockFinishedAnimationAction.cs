using UnityEngine;

namespace Loderunner.Gameplay
{
    public sealed class CharacterRemoveBlockFinishedAnimationAction: CharacterAnimationActionBase
    {
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
        }
    }
}