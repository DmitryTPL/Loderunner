using UnityEngine;

namespace Loderunner.Gameplay
{
    public class RestoreBlockAnimationAction : AnimationActionBase
    {
        private readonly bool _isRestoring;

        public RestoreBlockAnimationAction(bool isRestoring)
        {
            _isRestoring = isRestoring;
        }
        public override void Execute(Animator animator)
        {
            animator.SetBool(WallAnimationParameter.IsRestoring, _isRestoring);
        }
    }
}