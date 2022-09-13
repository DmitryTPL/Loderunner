using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public sealed class CharacterRemoveBlockAnimationAction: CharacterAnimationActionBase
    {
        public override UniTask Execute(Animator animator)
        {
            ResetAll(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsRemovingBlock, true);
            
            return UniTask.CompletedTask;
        }
    }
}