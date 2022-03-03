namespace Loderunner.Gameplay
{
    public readonly struct BorderReachedMessage
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