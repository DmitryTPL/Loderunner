using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public sealed class CharacterRemoveBlockFinishedAnimationAction: CharacterAnimationActionBase
    {
        public override UniTask Execute(Animator animator)
        {
            animator.SetBool(CharacterAnimationParameter.IsRemovingBlock, false);
            ResetAll(animator);
            
            return UniTask.CompletedTask;
        }
    }
}