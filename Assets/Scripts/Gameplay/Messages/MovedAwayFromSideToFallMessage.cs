namespace Loderunner.Gameplay
{
    public struct MovedAwayFromSideToFallMessage
    {
        public ICharacterView CharacterView { get; }
        public BorderType SideToFall { get; }

        public MovedAwayFromSideToFallMessage(ICharacterView characterView, BorderType sideToFall)
        {
            CharacterView = characterView;
            SideToFall = sideToFall;
        }
    }
}