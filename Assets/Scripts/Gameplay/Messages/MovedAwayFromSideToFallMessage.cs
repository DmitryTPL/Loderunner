namespace Loderunner.Gameplay
{
    public struct MovedAwayFromSideToFallMessage : IMessageForCharacter
    {
        public int CharacterId { get; }
        public BorderType SideToFall { get; }

        public MovedAwayFromSideToFallMessage(int characterId, BorderType sideToFall)
        {
            CharacterId = characterId;
            SideToFall = sideToFall;
        }
    }
}