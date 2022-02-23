using UnityEngine;

namespace Loderunner.Gameplay
{
    public struct StateResultData
    {
        public CharacterState CurrentState { get; }
        public Vector3 NextCharacterPosition { get; }
        
        public StateResultData(CharacterState currentState, Vector3 nextCharacterPosition)
        {
            CurrentState = currentState;
            NextCharacterPosition = nextCharacterPosition;
        }
    }
}