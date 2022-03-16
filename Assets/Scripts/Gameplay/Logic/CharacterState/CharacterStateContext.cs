using System.Collections.Generic;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class CharacterStateContext : ICharacterStateContext
    {
        private readonly GameConfig _gameConfig;

        private readonly Dictionary<int, CharacterStateBase> _states = new()
        {
            { (int)CharacterState.Moving, new MoveState() },
            { (int)CharacterState.CrossbarCrawling, new CrossbarCrawlingState() },
            { (int)CharacterState.LadderClimbing, new LadderClimbingState() },
            { (int)CharacterState.Falling, new FallingState() },
        };

        public CharacterStateContext(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }

        public StateResultData GetStateData(StateInitialData data)
        {
            var state = (CharacterState)0;
            var result = new StateResult(true);
            var previousState = state;

            while(result.MoveNext)
            {
                previousState = state;
                
                result = _states[(int)state++].Execute(data, _gameConfig);
            }

            return new StateResultData(previousState, result.NextCharacterPosition, result.MoveSpeed);
        }
    }
}