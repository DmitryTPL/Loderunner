using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LadderClimbingState : CharacterStateBase<StateData>
    {
        public LadderClimbingState(GameConfig gameConfig, ICharacterConfig characterConfig, StateData data)
            : base(gameConfig, characterConfig, data)
        {
        }

        public override StateResult Execute()
        {
            if (_data.ClimbingData.IsEmpty)
            {
                return new StateResult(true);
            }
            
            var moveSpeed = _data.MovingData.VerticalMove * _characterConfig.ClimbSpeed;

            var movement = new Vector2(0, moveSpeed * Time.deltaTime);

            var newPosition = _data.MovingData.CharacterPosition;

            if (!_data.MovingData.CharacterPosition.x.Equals(_data.ClimbingData.Center))
            {
                newPosition = new Vector2(_data.ClimbingData.Center, _data.MovingData.CharacterPosition.y);
            }

            newPosition += movement;

            // moving on ladder
            if (movement != Vector2.zero 
                && newPosition.y > _data.ClimbingData.Bottom 
                && newPosition.y < _data.ClimbingData.Top)
            {
                return new StateResult(newPosition, moveSpeed);
            }

            if (_data.PreviousState == CharacterState.LadderClimbing)
            {
                // climbing finished
                if (_data.MovingData.CharacterPosition.y >= _data.ClimbingData.Top
                    || _data.MovingData.CharacterPosition.y <= _data.ClimbingData.Bottom)
                {
                    return new StateResult(true);
                }

                // prevent climbing lower than ladder bottom
                if (_data.MovingData.VerticalMove < 0 && newPosition.y <= _data.ClimbingData.Bottom)
                {
                    return new StateResult(new Vector2(_data.MovingData.CharacterPosition.x, _data.ClimbingData.Bottom));
                }

                // prevent climbing higher than ladder top
                if (_data.MovingData.VerticalMove > 0 && newPosition.y >= _data.ClimbingData.Top)
                {
                    return new StateResult(new Vector2(_data.MovingData.CharacterPosition.x, _data.ClimbingData.Top));
                }

                // idle
                if (_data.MovingData.VerticalMove == 0
                    && _data.MovingData.HorizontalMove == 0
                    && _data.MovingData.CharacterPosition.y > _data.ClimbingData.Bottom
                    && _data.MovingData.CharacterPosition.y < _data.ClimbingData.Top)
                {
                    return new StateResult(_data.MovingData.CharacterPosition);
                }
            }

            return new StateResult(true);
        }
    }
}