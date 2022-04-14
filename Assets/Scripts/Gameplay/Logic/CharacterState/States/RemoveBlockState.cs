using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class RemoveBlockState : CharacterStateBase<PlayerStateData>
    {
        public RemoveBlockState(GameConfig gameConfig, ICharacterConfig characterConfig, PlayerStateData data)
            : base(gameConfig, characterConfig, data)
        {
        }

        public override StateResult Execute()
        {
            if (_data.RemoveBlockType == RemoveBlockType.None
                || !_data.IsGrounded
                || !_data.ClimbingData.IsEmpty && _data.PreviousState == CharacterState.LadderClimbing
                || !_data.CrawlingData.IsEmpty && _data.PreviousState == CharacterState.CrossbarCrawling)
            {
                return new StateResult(true);
            }

            var newPosition = _data.MovingData.CharacterPosition;

            if (!_data.MovingData.CharacterPosition.x.Equals(_data.RemoveBlockCharacterAlignedPosition.x))
            {
                var delta = _data.RemoveBlockCharacterAlignedPosition.x - _data.MovingData.CharacterPosition.x;

                var sign = Math.Sign(delta);

                delta = Math.Min(Math.Abs(delta), Time.deltaTime);

                newPosition = new Vector2(_data.MovingData.CharacterPosition.x + sign * delta,
                    _data.MovingData.CharacterPosition.y);
            }

            return new StateResult(newPosition);
        }
    }
}