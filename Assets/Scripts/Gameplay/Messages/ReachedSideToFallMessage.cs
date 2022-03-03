namespace Loderunner.Gameplay
{
    public struct ReachedSideToFallMessage
    {
        public ICharacterView CharacterView { get; }
        public float FallPoint { get; }
        public BorderType SideToFall { get; }

        public ReachedSideToFallMessage(ICharacterView characterView, float fallPoint, BorderType sideToFall)
        {
            CharacterView = characterView;
            FallPoint = fallPoint;
            SideToFall = sideToFall;
        }
    }
}