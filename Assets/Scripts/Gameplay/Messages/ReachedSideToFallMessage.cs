namespace Loderunner.Gameplay
{
    public readonly struct ReachedSideToFallMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public float FallPoint { get; }
        public SideToFallType SideToFall { get; }

        public ReachedSideToFallMessage(int characterId, float fallPoint, SideToFallType sideToFall)
        {
            CharacterId = characterId;
            FallPoint = fallPoint;
            SideToFall = sideToFall;
        }
    }
}