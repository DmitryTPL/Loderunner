namespace Loderunner.Gameplay
{
    public readonly struct CharacterCreatedMessage
    {
        public int CharacterId { get; }
        public CharacterType CharacterType { get; }
        
        public CharacterCreatedMessage(int characterId, CharacterType characterType)
        {
            CharacterId = characterId;
            CharacterType = characterType;
        }
    }
}