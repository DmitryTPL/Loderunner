namespace Loderunner.Gameplay
{
    public readonly struct FloorReachedMessage
    {
        public ICharacterView CharacterView { get; }
        public int ColliderId { get; }

        public FloorReachedMessage(ICharacterView characterView, int colliderId)
        {
            CharacterView = characterView;
            ColliderId = colliderId;
        }
    }
}