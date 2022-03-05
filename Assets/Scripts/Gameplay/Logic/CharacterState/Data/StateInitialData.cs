using UnityEngine;

namespace Loderunner.Gameplay
{
    public struct StateInitialData
    {
        public MovingData MovingData { get; }
        public ICharacterConfig CharacterConfig { get; }
        public ClimbingData ClimbingData { get; }
        public CrawlingData CrawlingData { get; }
        public CharacterState PreviousState { get; }
        public BorderType BorderReachedType { get; }
        public bool IsGrounded { get; }
        public float FallPoint { get; }

        public StateInitialData(MovingData movingData, ICharacterConfig characterConfig, 
            ClimbingData climbingData, CrawlingData crawlingData, CharacterState previousState, 
            BorderType borderReachedType, bool isGrounded, float fallPoint)
        {
            MovingData = movingData;
            CharacterConfig = characterConfig;
            ClimbingData = climbingData;
            CrawlingData = crawlingData;
            PreviousState = previousState;
            BorderReachedType = borderReachedType;
            IsGrounded = isGrounded;
            FallPoint = fallPoint;
        }
    }
}