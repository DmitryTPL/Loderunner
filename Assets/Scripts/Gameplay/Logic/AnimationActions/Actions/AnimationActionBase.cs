using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class AnimationActionBase
    {
        public abstract UniTask Execute(Animator animator);

        protected virtual void ResetAll(Animator animator)
        {
            
        }
    }
}