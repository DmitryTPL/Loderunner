using UnityEngine;

namespace Loderunner.Gameplay
{
    public readonly struct WallBlockRestoredMessage
    {
        public Vector2 WallBlockPosition { get; }

        public WallBlockRestoredMessage(Vector2 wallBlockPosition)
        {
            WallBlockPosition = wallBlockPosition;
        }
    }
}