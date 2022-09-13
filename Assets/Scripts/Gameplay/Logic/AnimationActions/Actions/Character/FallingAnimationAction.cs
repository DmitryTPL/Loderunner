using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class FallingAnimationAction : CharacterAnimationActionBase
    {
        public override UniTask Execute(Animator animator)
        {
            ResetAll(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsFalling, true);
            
            return UniTask.CompletedTask;
        }
    }
}