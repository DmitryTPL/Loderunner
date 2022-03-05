﻿using UnityEngine;

namespace Loderunner.Gameplay
{
    public class MoveAnimationAction : AnimationActionBase
    {
        private readonly bool _isMoving;

        public MoveAnimationAction(bool isMoving)
        {
            _isMoving = isMoving;
        }
        
        public override void Execute(Animator animator)
        {
            ResetAll(animator);
            
            animator.SetBool(CharacterAnimationParameter.IsMoving, _isMoving);
        }
    }
}