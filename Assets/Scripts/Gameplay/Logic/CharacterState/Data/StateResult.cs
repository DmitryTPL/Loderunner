using UnityEngine;

namespace Loderunner.Gameplay
{
    public struct StateResult
    {
        public bool MoveNext { get; }
        public Vector2 NextCharacterPosition { get; }

        public StateResult(Vector2 nextCharacterPosition)
        {
            MoveNext = false;
            NextCharacterPosition = nextCharacterPosition;
        }

        public StateResult(bool moveNext)
        {
            MoveNext = moveNext;
            NextCharacterPosition = Vector2.zero;
        }
    }
}