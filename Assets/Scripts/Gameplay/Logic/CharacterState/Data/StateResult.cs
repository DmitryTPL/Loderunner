namespace Loderunner.Gameplay
{
    public struct StateResult
    {
        public bool MoveNext { get; }
        public float MovementValue { get; }

        public StateResult(float movementValue)
        {
            MoveNext = false;
            MovementValue = movementValue;
        }
        
        public StateResult(bool moveNext)
        {
            MoveNext = moveNext;
            MovementValue = 0;
        }
    }
}