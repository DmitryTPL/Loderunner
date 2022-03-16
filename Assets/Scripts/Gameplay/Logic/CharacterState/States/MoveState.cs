using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class MoveState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            var moveSpeed = data.CharacterConfig.WalkSpeed * data.MovingData.HorizontalMove;

            var alignedPosition = data.MovingData.CharacterPosition;

            if (!data.MovingData.CharacterPosition.y.Equals(data.FloorPoint) && data.ClimbingData.IsEmpty)
            {
                alignedPosition = new Vector2(data.MovingData.CharacterPosition.x, data.FloorPoint);
            }

            var newPosition = alignedPosition + new Vector2(moveSpeed * Time.deltaTime, 0);

            var canMove = Math.Abs(data.MovingData.HorizontalMove) >= gameConfig.MovementThreshold &&
                          (data.MovingData.HorizontalMove > 0 && data.BorderReachedType != BorderType.Right ||
                           data.MovingData.HorizontalMove < 0 && data.BorderReachedType != BorderType.Left);

            return canMove ? new StateResult(newPosition, moveSpeed) : new StateResult(alignedPosition);
        }
    }
}