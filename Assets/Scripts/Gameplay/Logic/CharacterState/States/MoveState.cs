using System;

namespace Loderunner.Gameplay
{
    public class MoveState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            return Math.Abs(data.HorizontalMovement) >= gameConfig.MovementThreshold
                ? new StateResult(data.CharacterConfig.WalkSpeed * data.HorizontalMovement)
                : new StateResult(true);
        }
    }
}