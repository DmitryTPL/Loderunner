namespace Loderunner.Gameplay
{
    public readonly struct MovedAwayFromBorderMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public BorderType Border { get; }

        public MovedAwayFromBorderMessage(int characterId, BorderType border)
        {
            CharacterId = characterId;
            Border = border;
        }
    }
}