using System;

namespace Loderunner.Gameplay
{
    public class LadderClimbingIdleState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            if (data.IsClimbing && Math.Abs(data.VerticalMovement) < gameConfig.MovementThreshold)
            {
                if (data.CharacterPosition.y > data.ClimbingData.LadderBottom && 
                    data.CharacterPosition.y < data.ClimbingData.LadderTop)
                {
                    return new StateResult(0);
                }
            }

            return new StateResult(true);
        }
    }
}