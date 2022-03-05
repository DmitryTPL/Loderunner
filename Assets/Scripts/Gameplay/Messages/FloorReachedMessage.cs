namespace Loderunner.Gameplay
{
    public readonly struct FloorReachedMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public int FloorId { get; }

        public FloorReachedMessage(int characterId, int floorId)
        {
            CharacterId = characterId;
            FloorId = floorId;
        }
    }
}