using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LadderClimbingFinishedState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            if (data.IsClimbing)
            {
                var movement = new Vector3(0, data.VerticalMovement, 0);

                var newPosition = data.CharacterPosition + movement * Time.deltaTime;
                
                if (data.VerticalMovement < 0 && newPosition.y <= data.ClimbingData.LadderBottom || 
                    data.VerticalMovement > 0 && newPosition.y >= data.ClimbingData.LadderTop)
                {
                    return new StateResult(0);
                }
            }
            
            return new StateResult(true);
        }
    }
}