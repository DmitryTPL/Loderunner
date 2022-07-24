namespace Loderunner.Gameplay
{
    public readonly struct CharacterNeedToFallInRemovedBlockMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public float FallPoint { get; }
        public float Top { get; }

        public CharacterNeedToFallInRemovedBlockMessage(int characterId, float fallPoint, float top)
        {
            CharacterId = characterId;
            FallPoint = fallPoint;
            Top = top;
        }
    }
}