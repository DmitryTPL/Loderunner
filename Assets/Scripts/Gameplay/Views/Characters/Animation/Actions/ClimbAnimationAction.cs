using UnityEngine;

namespace Loderunner.Gameplay
{
    public class ClimbAnimationAction : AnimationActionBase
    {
        private readonly bool _isClimbing;

        public ClimbAnimationAction(bool isClimbing)
        {
            _isClimbing = isClimbing;
        }
        
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsClimbing, _isClimbing);
        }
    }
}