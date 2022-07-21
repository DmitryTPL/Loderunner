namespace Loderunner.Gameplay
{
    public readonly struct ExitCrossbarMessage : IMessageForCharacter
    {
        public int CharacterId { get; }

        public ExitCrossbarMessage(int characterId)
        {
            CharacterId = characterId;
        }
    }
}