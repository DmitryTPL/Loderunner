namespace Loderunner.Gameplay
{
    public readonly struct LevelCreatedMessage
    {
        public int LevelId { get; }
        
        public LevelCreatedMessage(int levelId)
        {
            LevelId = levelId;
        }
    }
}