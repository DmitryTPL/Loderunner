using System;

namespace Loderunner.Gameplay
{
    public readonly struct BorderReachedMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public Guid BorderId { get; }
        public BorderType Border { get; }
        public float TopPoint { get; }

        public BorderReachedMessage(int characterId, Guid borderId, BorderType border, float topPoint)
        {
            CharacterId = characterId;
            BorderId = borderId;
            Border = border;
            TopPoint = topPoint;
        }
    }
}