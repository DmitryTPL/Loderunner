namespace Loderunner.Gameplay
{
    public readonly struct GotOffTheFloorMessage
    {
        public ICharacterView CharacterView { get; }
        public int ColliderId { get; }

        public GotOffTheFloorMessage(ICharacterView characterView, int colliderId)
        {
            CharacterView = characterView;
            ColliderId = colliderId;
        }
    }
}