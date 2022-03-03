using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LadderClimbingState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            if (!data.ClimbingData.IsEmpty)
            {
                var movement = new Vector2(0, data.MovingData.VerticalMove * data.CharacterConfig.ClimbSpeed * Time.deltaTime);

                var newPosition = data.MovingData.CharacterPosition;

                if (!data.MovingData.CharacterPosition.x.Equals(data.ClimbingData.LadderCenter))
                {
                    newPosition = new Vector2(data.ClimbingData.LadderCenter, data.MovingData.CharacterPosition.y);
                }

                newPosition += movement;

                if (movement != Vector2.zero && newPosition.y > data.ClimbingData.LadderBottom &&
                    newPosition.y < data.ClimbingData.LadderTop)
                {
                    return new StateResult(newPosition);
                }

                if (data.PreviousState == CharacterState.LadderClimbing)
                {
                    if (data.MovingData.CharacterPosition.y >= data.ClimbingData.LadderTop ||
                        data.MovingData.CharacterPosition.y <= data.ClimbingData.LadderBottom)
                    {
                        return new StateResult(true);
                    }

                    if (data.MovingData.VerticalMove < 0 && newPosition.y <= data.ClimbingData.LadderBottom)
                    {
                        return new StateResult(new Vector2(data.MovingData.CharacterPosition.x, data.ClimbingData.LadderBottom));
                    }

                    if (data.MovingData.VerticalMove > 0 && newPosition.y >= data.ClimbingData.LadderTop)
                    {
                        return new StateResult(new Vector2(data.MovingData.CharacterPosition.x, data.ClimbingData.LadderTop));
                    }

                    if (data.MovingData.VerticalMove == 0)
                    {
                        if (data.MovingData.HorizontalMove == 0)
                        {
                            if (data.MovingData.CharacterPosition.y > data.ClimbingData.LadderBottom &&
                                data.MovingData.CharacterPosition.y < data.ClimbingData.LadderTop)
                            {
                                return new StateResult(data.MovingData.CharacterPosition);
                            }
                        }
                        else
                        {
                            return new StateResult(true);
                        }
                    }
                }
            }

            return new StateResult(true);
        }
    }
}