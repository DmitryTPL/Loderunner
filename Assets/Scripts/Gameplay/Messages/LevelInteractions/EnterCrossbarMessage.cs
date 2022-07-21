namespace Loderunner.Gameplay
{
    public readonly struct EnterCrossbarMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public CrawlingData CrawlingData { get; }

        public EnterCrossbarMessage(int characterId, float left, float right, float center)
        {
            CharacterId = characterId;
            CrawlingData = new CrawlingData(left, right, center);
        }
    }
}