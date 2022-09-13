using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class ClimbFinishedAnimationAction : CharacterAnimationActionBase
    {
        public override UniTask Execute(Animator animator)
        {
            ResetAll(animator);
            
            animator.SetTrigger(CharacterAnimationParameter.ClimbFinished);
            
            return UniTask.CompletedTask;
        }
    }
}