using UnityEngine;

namespace Loderunner.Gameplay
{
    public readonly struct WallBlockRestoringBeganMessage
    {
        public Vector2 WallBlockPosition { get; }

        public WallBlockRestoringBeganMessage(Vector2 wallBlockPosition)
        {
            WallBlockPosition = wallBlockPosition;
        }
    }
}