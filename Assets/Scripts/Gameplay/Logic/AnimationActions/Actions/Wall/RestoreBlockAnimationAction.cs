using Cysharp.Threading.Tasks;
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
        
        public override UniTask Execute(Animator animator)
        {
            animator.SetBool(WallAnimationParameter.IsRestoring, _isRestoring);

            return UniTask.CompletedTask;
        }
    }
}