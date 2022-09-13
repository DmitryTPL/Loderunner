using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class AnimationActionWithMoveSpeed : CharacterAnimationActionBase
    {
        private readonly float _moveSpeed;

        protected AnimationActionWithMoveSpeed(float moveSpeed)
        {
            _moveSpeed = Math.Abs(moveSpeed);
        }
        
        public override UniTask Execute(Animator animator)
        {
            animator.SetFloat(CharacterAnimationParameter.MoveSpeed, _moveSpeed);
            
            return UniTask.CompletedTask;
        }
    }
}