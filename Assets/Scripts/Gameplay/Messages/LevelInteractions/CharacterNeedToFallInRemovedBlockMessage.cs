namespace Loderunner.Gameplay
{
    public readonly struct CharacterNeedToFallInRemovedBlockMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public float FallPoint { get; }

        public CharacterNeedToFallInRemovedBlockMessage(int characterId, float fallPoint)
        {
            CharacterId = characterId;
            FallPoint = fallPoint;
        }
    }
}