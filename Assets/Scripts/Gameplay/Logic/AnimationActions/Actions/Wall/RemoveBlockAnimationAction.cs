using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class RemoveBlockAnimationAction : AnimationActionBase
    {
        private readonly bool _isRemoving;

        public RemoveBlockAnimationAction(bool isRemoving)
        {
            _isRemoving = isRemoving;
        }
        
        public override UniTask Execute(Animator animator)
        {
            animator.SetBool(WallAnimationParameter.IsRemoving, _isRemoving);
            
            return UniTask.CompletedTask;
        }
    }
}