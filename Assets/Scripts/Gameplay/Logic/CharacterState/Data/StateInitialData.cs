using UnityEngine;

namespace Loderunner.Gameplay
{
    public struct StateInitialData
    {
        public MovingData MovingData { get; }
        public ICharacterConfig CharacterConfig { get; }
        public ClimbingData ClimbingData { get; }
        public CharacterState PreviousState { get; }
        public BorderType BorderReachedType { get; }

        public StateInitialData(MovingData movingData, ICharacterConfig characterConfig, 
            ClimbingData climbingData, CharacterState previousState, BorderType borderReachedType)
        {
            MovingData = movingData;
            CharacterConfig = characterConfig;
            ClimbingData = climbingData;
            PreviousState = previousState;
            BorderReachedType = borderReachedType;
        }
    }
}