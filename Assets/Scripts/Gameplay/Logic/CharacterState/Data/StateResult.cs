using UnityEngine;

namespace Loderunner.Gameplay
{
    public struct StateResult
    {
        public bool MoveNext { get; }
        public Vector3 NextCharacterPosition { get; }

        public StateResult(Vector3 nextCharacterPosition)
        {
            MoveNext = false;
            NextCharacterPosition = nextCharacterPosition;
        }

        public StateResult(bool moveNext)
        {
            MoveNext = moveNext;
            NextCharacterPosition = Vector3.zero;
        }
    }
}