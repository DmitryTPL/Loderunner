namespace Loderunner.Gameplay
{
    public readonly struct StopActingMessage : IMessageForCharacter
    {
        public int CharacterId { get; }

        public StopActingMessage(int guardianId)
        {
            CharacterId = guardianId;
        }
    }
}