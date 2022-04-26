using UnityEngine;

namespace Loderunner.Gameplay
{
    public readonly struct PlayerMovedMessage
    {
        public Vector2 Position { get; }

        public PlayerMovedMessage (Vector2 position)
        {
            Position = position;
        }
    }
}