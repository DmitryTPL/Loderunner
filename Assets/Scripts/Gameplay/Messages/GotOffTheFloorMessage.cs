namespace Loderunner.Gameplay
{
    public readonly struct GotOffTheFloorMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public int FloorId { get; }

        public GotOffTheFloorMessage(int characterId, int floorId)
        {
            CharacterId = characterId;
            FloorId = floorId;
        }
    }
}