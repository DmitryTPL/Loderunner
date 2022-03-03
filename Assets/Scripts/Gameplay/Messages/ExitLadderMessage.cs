namespace Loderunner.Gameplay
{
    public readonly struct ExitLadderMessage
    {
        public ICharacterView CharacterView { get; }

        public ExitLadderMessage(ICharacterView characterView)
        {
            CharacterView = characterView;
        }
    }
}