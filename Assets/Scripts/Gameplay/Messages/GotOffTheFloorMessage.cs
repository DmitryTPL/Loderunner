namespace Loderunner.Gameplay
{
    public readonly struct GotOffTheFloorMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public int FloorId { get; }
        public IWallBlocksHolder WallBlocksHolder { get; }

        public GotOffTheFloorMessage(int characterId, int floorId, IWallBlocksHolder wallBlocksHolder)
        {
            CharacterId = characterId;
            FloorId = floorId;
            WallBlocksHolder = wallBlocksHolder;
        }
    }
}