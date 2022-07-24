using System;

namespace Loderunner.Gameplay
{
    public readonly struct GotOffTheRemovedWallGuardianGroundMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public Guid Guid { get; }

        public GotOffTheRemovedWallGuardianGroundMessage(int characterId, Guid guid)
        {
            CharacterId = characterId;
            Guid = guid;
        }
    }
}