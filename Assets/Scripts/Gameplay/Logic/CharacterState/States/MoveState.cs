using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class MoveState : CharacterStateBase<StateData>
    {
        public MoveState(GameConfig gameConfig, ICharacterConfig characterConfig, StateData data) 
            : base(gameConfig, characterConfig, data)
        {
        }

        public override StateResult Execute()
        {
            var moveSpeed = _characterConfig.WalkSpeed * _data.MovingData.HorizontalMove;

            var alignedPosition = _data.MovingData.CharacterPosition;

            if (!_data.MovingData.CharacterPosition.y.Equals(_data.FloorPoint) && _data.ClimbingData.IsEmpty)
            {
                alignedPosition = new Vector2(_data.MovingData.CharacterPosition.x, _data.FloorPoint);
            }

            var newPosition = alignedPosition + new Vector2(moveSpeed * Time.deltaTime, 0);

            var canMove = Math.Abs(_data.MovingData.HorizontalMove) >= _gameConfig.MovementThreshold &&
                          (_data.MovingData.HorizontalMove > 0 && _data.BorderReachedType != BorderType.Right ||
                           _data.MovingData.HorizontalMove < 0 && _data.BorderReachedType != BorderType.Left);

            return canMove ? new StateResult(newPosition, moveSpeed) : new StateResult(alignedPosition);
        }
    }
}