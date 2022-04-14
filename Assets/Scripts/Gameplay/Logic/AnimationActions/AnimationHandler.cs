using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class AnimationHandler : MonoBehaviour
    {
        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void ApplyAnimation(AnimationActionBase animationAction)
        {
            animationAction?.Execute(_animator);
        }
    }
}