namespace Loderunner.Gameplay
{
    public struct BorderReachedMessage
    {
        public ICharacterView CharacterView { get; }
        public BorderType BorderType { get; }

        public BorderReachedMessage(ICharacterView characterView, BorderType borderType)
        {
            CharacterView = characterView;
            BorderType = borderType;
        }
    }
}