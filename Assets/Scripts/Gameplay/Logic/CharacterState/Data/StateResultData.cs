using UnityEngine;

namespace Loderunner.Gameplay
{
    public struct StateResultData
    {
        public CharacterState CurrentState { get; }
        public Vector3 NextCharacterPosition { get; }
        public float MoveSpeed { get; }
        
        public StateResultData(CharacterState currentState, Vector3 nextCharacterPosition, float moveSpeed)
        {
            CurrentState = currentState;
            NextCharacterPosition = nextCharacterPosition;
            MoveSpeed = moveSpeed;
        }
    }
}