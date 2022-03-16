namespace Loderunner.Gameplay
{
    public readonly struct FloorReachedMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public int FloorId { get; }
        public float FloorPoint { get; }

        public FloorReachedMessage(int characterId, int floorId, float floorPoint)
        {
            CharacterId = characterId;
            FloorId = floorId;
            FloorPoint = floorPoint;
        }
    }
}