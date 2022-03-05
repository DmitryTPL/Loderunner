namespace Loderunner.Gameplay
{
    public readonly struct MovedAwayFromBorderMessage : IMessageForCharacter
    {
        public int CharacterId { get; }

        public MovedAwayFromBorderMessage(int characterId)
        {
            CharacterId = characterId;
        }
    }
}