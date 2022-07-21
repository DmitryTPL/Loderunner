using UnityEngine;

namespace Loderunner.Gameplay
{
    public readonly struct WallBlockRemovingBeganMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public Vector2 WallBlockPosition { get; }

        public WallBlockRemovingBeganMessage(int characterId, Vector2 wallBlockPosition)
        {
            CharacterId = characterId;
            WallBlockPosition = wallBlockPosition;
        }
    }
}