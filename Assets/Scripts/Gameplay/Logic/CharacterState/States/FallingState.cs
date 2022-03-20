using UnityEngine;

namespace Loderunner.Gameplay
{
    public class FallingState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            if (data.IsGrounded || !data.ClimbingData.IsEmpty)
            {
                return new StateResult(true);
            }
            
            var newPosition = new Vector2(data.FallPoint, data.MovingData.CharacterPosition.y);
                
            if (data.PreviousState == CharacterState.CrossbarCrawling)
            {
                newPosition = data.MovingData.CharacterPosition;
            }
                
            var movement = new Vector2(0, -data.CharacterConfig.FallSpeed * Time.deltaTime);

            return new StateResult(newPosition + movement);

        }
    }
}