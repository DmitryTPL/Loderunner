using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class AnimationActionWithMoveSpeed : AnimationActionBase
    {
        private readonly float _moveSpeed;

        protected AnimationActionWithMoveSpeed(float moveSpeed)
        {
            _moveSpeed = Math.Abs(moveSpeed);
        }
        
        public override void Execute(Animator animator)
        {
            animator.SetFloat(CharacterAnimationParameter.MoveSpeed, _moveSpeed);
        }
    }
}