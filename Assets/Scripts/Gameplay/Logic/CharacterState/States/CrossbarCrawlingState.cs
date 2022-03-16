using System;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class CrossbarCrawlingState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            if (!data.CrawlingData.IsEmpty)
            {
                if (data.CrawlingData.IsFinished || data.MovingData.VerticalMove < 0)
                {
                    return new StateResult(true);
                }

                var moveSpeed = data.MovingData.HorizontalMove * data.CharacterConfig.CrawlSpeed;
                
                var movement = new Vector2(moveSpeed * Time.deltaTime, 0);

                var alignedPosition = data.MovingData.CharacterPosition;
                    
                if (!data.MovingData.CharacterPosition.y.Equals(data.CrawlingData.Center))
                {
                    alignedPosition = new Vector2(data.MovingData.CharacterPosition.x, data.CrawlingData.Center);
                }
                
                var newPosition = alignedPosition + movement;

                var canMove = Math.Abs(data.MovingData.HorizontalMove) >= gameConfig.MovementThreshold &&
                              (data.MovingData.HorizontalMove > 0 && data.BorderReachedType != BorderType.Right ||
                               data.MovingData.HorizontalMove < 0 && data.BorderReachedType != BorderType.Left);

                if (!canMove)
                {
                    return new StateResult(alignedPosition);
                }

                if (movement != Vector2.zero && newPosition.x > data.CrawlingData.Left &&
                    newPosition.x < data.CrawlingData.Right)
                {
                    return new StateResult(newPosition, moveSpeed);
                }

                if (data.PreviousState == CharacterState.CrossbarCrawling 
                    && data.MovingData.HorizontalMove == 0 && data.MovingData.VerticalMove == 0
                    && data.MovingData.CharacterPosition.x > data.CrawlingData.Left 
                    && data.MovingData.CharacterPosition.x < data.CrawlingData.Right)
                {
                    // idle
                    return new StateResult(data.MovingData.CharacterPosition);
                }
            }

            return new StateResult(true);
        }
    }
}