using UnityEngine;

namespace Loderunner.Gameplay
{
    public class FallingState : CharacterStateBase<StateData>
    {
        public FallingState(GameConfig gameConfig, ICharacterConfig characterConfig, StateData data)
            : base(gameConfig, characterConfig, data)
        {
        }

        public override StateResult Execute()
        {
            if (_data.IsGrounded || !_data.ClimbingData.IsEmpty)
            {
                return new StateResult(true);
            }

            var newPosition = new Vector2(_data.FallPoint, _data.MovingData.CharacterPosition.y);

            if (_data.PreviousState == CharacterState.CrossbarCrawling)
            {
                newPosition = _data.MovingData.CharacterPosition;
            }

            var movement = new Vector2(0, -_characterConfig.FallSpeed * Time.deltaTime);

            return new StateResult(newPosition + movement);
        }
    }
}