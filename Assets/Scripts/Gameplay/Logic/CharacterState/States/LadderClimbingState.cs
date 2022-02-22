using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LadderClimbingState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            if (!data.ClimbingData.IsEmpty && Math.Abs(data.VerticalMovement) > 0)
            {
                var movement = new Vector3(0, data.VerticalMovement * data.CharacterConfig.ClimbSpeed * Time.deltaTime, 0);

                var newPosition = data.CharacterPosition + movement;

                if (newPosition.y > data.ClimbingData.LadderBottom &&
                    newPosition.y < data.ClimbingData.LadderTop)
                {
                    return new StateResult(movement.y);
                }
            }

            return new StateResult(true);
        }
    }
}