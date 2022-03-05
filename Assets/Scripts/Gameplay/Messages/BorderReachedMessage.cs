namespace Loderunner.Gameplay
{
    public readonly struct BorderReachedMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public BorderType Border { get; }

        public BorderReachedMessage(int characterId, BorderType border)
        {
            CharacterId = characterId;
            Border = border;
        }
    }
}