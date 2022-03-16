using UnityEngine;

namespace Loderunner.Gameplay
{
    public struct StateResult
    {
        public bool MoveNext { get; }
        public Vector2 NextCharacterPosition { get; }
        public float MoveSpeed { get; }

        public StateResult(Vector2 nextCharacterPosition, float moveSpeed = 0)
        {
            MoveNext = false;
            NextCharacterPosition = nextCharacterPosition;
            MoveSpeed = moveSpeed;
        }

        public StateResult(bool moveNext)
        {
            MoveNext = moveNext;
            MoveSpeed = 0;
            NextCharacterPosition = Vector2.zero;
        }
    }
}