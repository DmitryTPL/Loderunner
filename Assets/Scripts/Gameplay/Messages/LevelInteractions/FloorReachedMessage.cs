using System;

namespace Loderunner.Gameplay
{
    public readonly struct FloorReachedMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public Guid FloorId { get; }
        public float TopPoint { get; }
        public IWallBlocksHolder WallBlocksHolder { get; }

        public FloorReachedMessage(int characterId, Guid floorId, float topPoint, IWallBlocksHolder wallBlocksHolder)
        {
            CharacterId = characterId;
            FloorId = floorId;
            TopPoint = topPoint;
            WallBlocksHolder = wallBlocksHolder;
        }
    }
}