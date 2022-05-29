using System.Collections.Generic;

namespace Loderunner.Gameplay
{
    public abstract class CharacterStateContext<TData> : ICharacterStateContext
        where TData: StateData
    {
        private readonly GameConfig _gameConfig;
        private readonly ICharacterConfig _characterConfig;

        protected Dictionary<int, IExecuteState> States { get; }

        public TData StateData { get; }
        
        protected CharacterStateContext(GameConfig gameConfig, ICharacterConfig characterConfig, TData stateData)
        {
            StateData = stateData;
            
            States = new()
            {
                { (int)CharacterState.Moving, new MoveState(gameConfig, characterConfig, stateData) },
                { (int)CharacterState.CrossbarCrawling, new CrossbarCrawlingState(gameConfig, characterConfig, stateData) },
                { (int)CharacterState.LadderClimbing, new LadderClimbingState(gameConfig, characterConfig, stateData) },
                { (int)CharacterState.Falling, new FallingState(gameConfig, characterConfig, stateData) }
            };
        }

        public UpdatedStateData GetStateData()
        {
            var state = (CharacterState)0;
            var result = new StateResult(true);
            var previousState = state;

            while(result.MoveNext)
            {
                if (!States.ContainsKey((int)state))
                {
                    state++;
                    continue;
                }
                
                previousState = state;
                
                result = States[(int)state++].Execute();
            }

            return new UpdatedStateData(previousState, result.NextCharacterPosition, result.MoveSpeed);
        }
    }
}