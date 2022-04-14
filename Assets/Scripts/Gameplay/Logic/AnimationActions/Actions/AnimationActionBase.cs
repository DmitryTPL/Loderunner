using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class AnimationActionBase
    {
        public abstract void Execute(Animator animator);

        protected virtual void ResetAll(Animator animator)
        {
            
        }
    }
}