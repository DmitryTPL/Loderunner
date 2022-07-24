using System;

namespace Loderunner.Gameplay
{
    public readonly struct RemovedWallGuardianGroundReachedMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public Guid Guid { get; }
        public float TopPoint { get; }

        public RemovedWallGuardianGroundReachedMessage(int characterId, Guid guid, float topPoint)
        {
            CharacterId = characterId;
            Guid = guid;
            TopPoint = topPoint;
        }
    }
}