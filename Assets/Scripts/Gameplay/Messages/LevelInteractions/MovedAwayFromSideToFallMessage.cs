namespace Loderunner.Gameplay
{
    public struct MovedAwayFromSideToFallMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public SideToFallType SideToFall { get; }

        public MovedAwayFromSideToFallMessage(int characterId, SideToFallType sideToFall)
        {
            CharacterId = characterId;
            SideToFall = sideToFall;
        }
    }
}