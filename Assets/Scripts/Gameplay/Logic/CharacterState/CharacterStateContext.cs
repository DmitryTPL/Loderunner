using System.Collections.Generic;

namespace Loderunner.Gameplay
{
    public abstract class CharacterStateContext<TData> : ICharacterStateContext
        where TData: StateData
    {
        private readonly GameConfig _gameConfig;
        private readonly ICharacterConfig _characterConfig;

        protected Dictionary<int, IExecuteState> States { get; set; }

        public TData StateData { get; }
        
        protected CharacterStateContext(TData stateData)
        {
            StateData = stateData;
        }

        public UpdatedStateData GetStateData()
        {
            var state = (CharacterState)0;
            var result = new StateResult(true);
            var previousState = state;

            while(result.MoveNext)
            {
                previousState = state;
                
                result = States[(int)state++].Execute();
            }

            return new UpdatedStateData(previousState, result.NextCharacterPosition, result.MoveSpeed);
        }
    }
}