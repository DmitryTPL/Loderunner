using System;

namespace Loderunner.Gameplay
{
    public readonly struct CharacterCollectGoldMessage: IMessageForCharacter
    {
        public Guid GoldGuid { get; }
        public int CharacterId { get; }

        public CharacterCollectGoldMessage(Guid goldGuid, int characterId)
        {
            GoldGuid = goldGuid;
            CharacterId = characterId;
        }
    }
}