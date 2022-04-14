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
        
        public override void Execute(Animator animator)
        {
            animator.SetBool(WallAnimationParameter.IsRemoving, _isRemoving);
        }
    }
}