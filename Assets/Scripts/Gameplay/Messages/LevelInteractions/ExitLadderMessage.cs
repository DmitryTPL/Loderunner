namespace Loderunner.Gameplay
{
    public readonly struct ExitLadderMessage: IMessageForCharacter
    {
        public int CharacterId { get; }

        public ExitLadderMessage(int characterId)
        {
            CharacterId = characterId;
        }
    }
}