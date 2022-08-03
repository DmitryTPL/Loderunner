namespace Loderunner.Gameplay
{
    public readonly struct RespawnGuardianMessage : IMessageForCharacter
    {
        public int CharacterId { get; }

        public RespawnGuardianMessage(int characterId)
        {
            CharacterId = characterId;
        }
    }
}