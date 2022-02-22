using System;

namespace Loderunner.Gameplay
{
    public class MoveIdleState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            return Math.Abs(data.HorizontalMovement) < gameConfig.MovementThreshold ? new StateResult(0) : new StateResult(true);
        }
    }
}