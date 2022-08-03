namespace Loderunner.Gameplay
{
    public readonly struct GuardianRespawnedMessage : IMessageForCharacter
    {
        public int CharacterId { get; }

        public GuardianRespawnedMessage(int characterId)
        {
            CharacterId = characterId;
        }
    }
}