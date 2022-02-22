namespace Loderunner.Gameplay
{
    public struct StateResultData
    {
        public State CurrentState { get; }
        public float MovementValue { get; }
        
        public StateResultData(State currentState, float movementValue)
        {
            CurrentState = currentState;
            MovementValue = movementValue;
        }
    }
}