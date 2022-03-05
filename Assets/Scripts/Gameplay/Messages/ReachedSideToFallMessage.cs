namespace Loderunner.Gameplay
{
    public struct ReachedSideToFallMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public float FallPoint { get; }
        public BorderType SideToFall { get; }

        public ReachedSideToFallMessage(int characterId, float fallPoint, BorderType sideToFall)
        {
            CharacterId = characterId;
            FallPoint = fallPoint;
            SideToFall = sideToFall;
        }
    }
}