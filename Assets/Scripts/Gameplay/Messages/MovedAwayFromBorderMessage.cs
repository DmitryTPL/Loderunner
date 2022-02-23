namespace Loderunner.Gameplay
{
    public struct MovedAwayFromBorderMessage
    {
        public ICharacterView CharacterView { get; }
        public BorderType BorderType { get; }

        public MovedAwayFromBorderMessage(ICharacterView characterView, BorderType borderType)
        {
            CharacterView = characterView;
            BorderType = borderType;
        }
    }
}