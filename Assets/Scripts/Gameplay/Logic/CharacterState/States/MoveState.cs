using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class MoveState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            var newPosition = data.MovingData.CharacterPosition +
                              new Vector2(data.CharacterConfig.WalkSpeed * data.MovingData.HorizontalMove * Time.deltaTime, 0);

            var canMove = Math.Abs(data.MovingData.HorizontalMove) >= gameConfig.MovementThreshold &&
                          (data.MovingData.HorizontalMove > 0 && data.BorderReachedType != BorderType.Right ||
                           data.MovingData.HorizontalMove < 0 && data.BorderReachedType != BorderType.Left);

            return canMove
                ? new StateResult(newPosition)
                : new StateResult(data.MovingData.CharacterPosition);
        }
    }
}