using System;

namespace Loderunner.Gameplay
{
    public readonly struct MovedAwayFromBorderMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public Guid BorderId { get; }
        public BorderType Border { get; }

        public MovedAwayFromBorderMessage(int characterId, Guid borderId, BorderType border)
        {
            CharacterId = characterId;
            BorderId = borderId;
            Border = border;
        }
    }
}