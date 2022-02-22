using UnityEngine;

namespace Loderunner.Gameplay
{
    public struct StateInitialData
    {
        public float VerticalMovement { get; }
        public float HorizontalMovement { get; }
        public ICharacterConfig CharacterConfig { get; }
        public ClimbingData ClimbingData { get; }
        public Vector3 CharacterPosition { get; }
        public bool IsClimbing { get; }

        public StateInitialData(float horizontalMovement, float verticalMovement, ICharacterConfig characterConfig, 
            ClimbingData climbingData, Vector3 characterPosition, bool isClimbing)
        {
            VerticalMovement = verticalMovement;
            HorizontalMovement = horizontalMovement;
            CharacterConfig = characterConfig;
            ClimbingData = climbingData;
            CharacterPosition = characterPosition;
            IsClimbing = isClimbing;
        }
    }
}