using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class CrossbarCrawlingState : CharacterStateBase<StateData>
    {
        public CrossbarCrawlingState(GameConfig gameConfig, ICharacterConfig characterConfig, StateData data)
            : base(gameConfig, characterConfig, data)
        {
        }

        public override StateResult Execute()
        {
            if (_data.CrawlingData.IsEmpty || _data.CrawlingData.IsFinished
                                           || _data.MovingData.VerticalMove < 0 && !_data.IsGrounded)
            {
                return new StateResult(true);
            }

            var moveSpeed = _data.MovingData.HorizontalMove * _characterConfig.CrawlSpeed;

            var movement = new Vector2(moveSpeed * Time.deltaTime, 0);

            var alignedPosition = _data.MovingData.CharacterPosition;

            if (!_data.MovingData.CharacterPosition.y.Equals(_data.CrawlingData.Center))
            {
                alignedPosition = new Vector2(_data.MovingData.CharacterPosition.x, _data.CrawlingData.Center);
            }

            var newPosition = alignedPosition + movement;

            var canMove = Math.Abs(_data.MovingData.HorizontalMove) >= _gameConfig.MovementThreshold &&
                          (_data.MovingData.HorizontalMove > 0 && _data.BorderReachedType != BorderType.Right ||
                           _data.MovingData.HorizontalMove < 0 && _data.BorderReachedType != BorderType.Left);

            if (!canMove)
            {
                return new StateResult(alignedPosition);
            }

            if (movement != Vector2.zero && newPosition.x > _data.CrawlingData.Left && newPosition.x < _data.CrawlingData.Right)
            {
                return new StateResult(newPosition, moveSpeed);
            }

            if (_data.PreviousState == CharacterState.CrossbarCrawling
                && _data.MovingData.HorizontalMove == 0 && _data.MovingData.VerticalMove == 0
                && _data.MovingData.CharacterPosition.x > _data.CrawlingData.Left
                && _data.MovingData.CharacterPosition.x < _data.CrawlingData.Right)
            {
                // idle
                return new StateResult(_data.MovingData.CharacterPosition);
            }

            return new StateResult(true);
        }
    }
}