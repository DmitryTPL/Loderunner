namespace Loderunner.Gameplay
{
    public abstract class CharacterStateBase
    {
        public abstract StateResult Execute(StateInitialData data, GameConfig gameConfig);
    }
}