using System;

namespace Loderunner.Gameplay
{
    public readonly struct GotOffTheFloorMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public Guid FloorId { get; }
        public IWallBlocksHolder WallBlocksHolder { get; }

        public GotOffTheFloorMessage(int characterId, Guid floorId, IWallBlocksHolder wallBlocksHolder)
        {
            CharacterId = characterId;
            FloorId = floorId;
            WallBlocksHolder = wallBlocksHolder;
        }
    }
}