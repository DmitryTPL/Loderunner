using UnityEngine;

namespace Loderunner.Gameplay
{
    public struct MovingData
    {
        public float HorizontalMove { get; }
        public float VerticalMove { get; }
        public Vector3 CharacterPosition { get; }

        public MovingData(float horizontalMove, float verticalMove, Vector3 characterPosition)
        {
            HorizontalMove = horizontalMove;
            VerticalMove = verticalMove;
            CharacterPosition = characterPosition;
        }
    }
}