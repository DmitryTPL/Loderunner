using System;
using Cysharp.Threading.Tasks;
using UniTaskPubSub;

namespace Loderunner.Gameplay
{
    public class FallingOpportunityObserver : IFallPointHolder
    {
        public float FallPoint
        {
            get
            {
                var fallingPosition = 0f;

                if (Math.Abs(_rightFallPoint) >= float.Epsilon)
                {
                    fallingPosition = _rightFallPoint;
                }
                else if (Math.Abs(_leftFallPoint) >= float.Epsilon)
                {
                    fallingPosition = _leftFallPoint;
                }
                else if (Math.Abs(_bottomFallPoint) >= float.Epsilon)
                {
                    fallingPosition = _bottomFallPoint;
                }

                return fallingPosition;
            }
        }

        private float _leftFallPoint;
        private float _rightFallPoint;
        private float _bottomFallPoint;
        
        public FallingOpportunityObserver(IAsyncSubscriber subscriber)
        {
            subscriber.Subscribe<ReachedSideToFallMessage>(OnReachedSideToFall);
            subscriber.Subscribe<MovedAwayFromSideToFallMessage>(OnMovedAwayFromSideToFall);
        }

        private UniTask OnReachedSideToFall(ReachedSideToFallMessage message)
        {
            switch (message.SideToFall)
            {
                case BorderType.Left:
                    _leftFallPoint = message.FallPoint;
                    break;
                case BorderType.Right:
                    _rightFallPoint = message.FallPoint;
                    break;
                case BorderType.Bottom:
                    _bottomFallPoint = message.FallPoint;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return UniTask.CompletedTask;
        }
        
        private UniTask OnMovedAwayFromSideToFall(MovedAwayFromSideToFallMessage message)
        {
            switch (message.SideToFall)
            {
                case BorderType.Left:
                    _leftFallPoint = 0;
                    break;
                case BorderType.Right:
                    _rightFallPoint = 0;
                    break;
                case BorderType.Bottom:
                    _bottomFallPoint = 0;
                    break;
            }

            return UniTask.CompletedTask;
        }
    }
}