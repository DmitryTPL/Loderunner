using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LadderClimbingState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            if (!data.ClimbingData.IsEmpty)
            {
                var moveSpeed = data.MovingData.VerticalMove * data.CharacterConfig.ClimbSpeed;
                
                var movement = new Vector2(0, moveSpeed * Time.deltaTime);

                var newPosition = data.MovingData.CharacterPosition;

                if (!data.MovingData.CharacterPosition.x.Equals(data.ClimbingData.Center))
                {
                    newPosition = new Vector2(data.ClimbingData.Center, data.MovingData.CharacterPosition.y);
                }

                newPosition += movement;

                // moving on ladder
                if (movement != Vector2.zero && newPosition.y > data.ClimbingData.Bottom &&
                    newPosition.y < data.ClimbingData.Top)
                {
                    return new StateResult(newPosition, moveSpeed);
                }

                if (data.PreviousState == CharacterState.LadderClimbing)
                {
                    // climbing finished
                    if (data.MovingData.CharacterPosition.y >= data.ClimbingData.Top ||
                        data.MovingData.CharacterPosition.y <= data.ClimbingData.Bottom)
                    {
                        return new StateResult(true);
                    }

                    // prevent climbing lower than ladder bottom
                    if (data.MovingData.VerticalMove < 0 && newPosition.y <= data.ClimbingData.Bottom)
                    {
                        return new StateResult(new Vector2(data.MovingData.CharacterPosition.x, data.ClimbingData.Bottom));
                    }

                    // prevent climbing higher than ladder top
                    if (data.MovingData.VerticalMove > 0 && newPosition.y >= data.ClimbingData.Top)
                    {
                        return new StateResult(new Vector2(data.MovingData.CharacterPosition.x, data.ClimbingData.Top));
                    }

                    // idle
                    if (data.MovingData.VerticalMove == 0 
                        && data.MovingData.HorizontalMove == 0 
                        && data.MovingData.CharacterPosition.y > data.ClimbingData.Bottom 
                        && data.MovingData.CharacterPosition.y < data.ClimbingData.Top)
                    {
                        return new StateResult(data.MovingData.CharacterPosition);
                    }
                }
            }

            return new StateResult(true);
        }
    }
}