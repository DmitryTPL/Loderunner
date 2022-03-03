namespace Loderunner.Gameplay
{
    public readonly struct MovedAwayFromBorderMessage
    {
        public ICharacterView CharacterView { get; }

        public MovedAwayFromBorderMessage(ICharacterView characterView)
        {
            CharacterView = characterView;
        }
    }
}