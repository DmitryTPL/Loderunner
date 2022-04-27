using System;

namespace Loderunner.Gameplay
{
    public readonly struct CharacterReachedGoldMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public Guid GoldGuid { get; }

        public CharacterReachedGoldMessage(int characterId, Guid goldGuid)
        {
            CharacterId = characterId;
            GoldGuid = goldGuid;
        }
    }
}