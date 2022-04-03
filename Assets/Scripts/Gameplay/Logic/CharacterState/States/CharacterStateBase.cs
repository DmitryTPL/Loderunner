namespace Loderunner.Gameplay
{
    public abstract class CharacterStateBase<T> : IExecuteState
        where T: StateData
    {
        protected readonly GameConfig _gameConfig;
        protected readonly ICharacterConfig _characterConfig;
        protected readonly T _data;

        protected CharacterStateBase(GameConfig gameConfig, ICharacterConfig characterConfig, T data)
        {
            _gameConfig = gameConfig;
            _characterConfig = characterConfig;
            _data = data;
        }
        
        public abstract StateResult Execute();
    }
}