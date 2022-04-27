using System;

namespace Loderunner.Gameplay
{
    public readonly struct CharacterTookGoldMessage: IMessageForCharacter
    {
        public Guid GoldGuid { get; }
        public int CharacterId { get; }

        public CharacterTookGoldMessage(Guid goldGuid, int characterId)
        {
            GoldGuid = goldGuid;
            CharacterId = characterId;
        }
    }
}