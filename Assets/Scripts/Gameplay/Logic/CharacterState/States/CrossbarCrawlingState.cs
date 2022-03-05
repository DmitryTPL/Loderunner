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
                if (data.CrawlingData.IsFinished)
                {
                    return new StateResult(true);
                }
                
                var movement = new Vector2(data.MovingData.HorizontalMove * data.CharacterConfig.CrawlSpeed * Time.deltaTime, 0);

                var newPosition = data.MovingData.CharacterPosition + movement;

                var canMove = Math.Abs(data.MovingData.HorizontalMove) >= gameConfig.MovementThreshold &&
                              (data.MovingData.HorizontalMove > 0 && data.BorderReachedType != BorderType.Right ||
                               data.MovingData.HorizontalMove < 0 && data.BorderReachedType != BorderType.Left);

                if (!canMove)
                {
                    return new StateResult(data.MovingData.CharacterPosition);
                }

                if (movement != Vector2.zero && newPosition.x > data.CrawlingData.Left &&
                    newPosition.x < data.CrawlingData.Right)
                {
                    return new StateResult(newPosition);
                }

                if (data.PreviousState == CharacterState.CrossbarCrawling)
                {
                    // idle
                    if (data.MovingData.HorizontalMove == 0 && data.MovingData.VerticalMove == 0)
                    {
                        if (data.MovingData.CharacterPosition.x > data.CrawlingData.Left &&
                            data.MovingData.CharacterPosition.x < data.CrawlingData.Right)
                        {
                            return new StateResult(data.MovingData.CharacterPosition);
                        }
                    }
                }
            }

            return new StateResult(true);
        }
    }
}