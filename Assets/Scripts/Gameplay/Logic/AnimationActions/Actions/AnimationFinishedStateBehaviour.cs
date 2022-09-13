using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class AnimationFinishedStateBehaviour : StateMachineBehaviour
    {
        public event Action Finished;
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Finished?.Invoke();
        }
    }
}