namespace Loderunner.Gameplay
{
    public struct PlayerExitLadderMessage
    {
        public ICharacterView CharacterView { get; }

        public PlayerExitLadderMessage(ICharacterView characterView)
        {
            CharacterView = characterView;
        }
    }
}