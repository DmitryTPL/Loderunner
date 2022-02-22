using System.Collections.Generic;
using UniTaskPubSub;

namespace Loderunner.Gameplay
{
    public class CharacterStateContext : ICharacterStateContext
    {
        private readonly GameConfig _gameConfig;
        private readonly AsyncMessageBus _messageBus;

        private readonly Dictionary<int, CharacterStateBase> _states = new()
        {
            { (int)State.Moving, new MoveState() },
            { (int)State.Idle, new MoveIdleState() },
            { (int)State.CrossbarCrawling, new CrossbarCrawlingState() },
            { (int)State.CrossbarCrawlingIdle, new CrossbarCrawlingIdleState() },
            { (int)State.LadderClimbing, new LadderClimbingState() },
            { (int)State.LadderClimbingIdle, new LadderClimbingIdleState() },
            { (int)State.LadderClimbingFinished, new LadderClimbingFinishedState() },
        };

        public CharacterStateContext(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }

        public StateResultData GetStateData(StateInitialData data)
        {
            var state = (State)0;
            var result = new StateResult(true);
            var previousState = state;

            while(result.MoveNext)
            {
                previousState = state;
                
                result = _states[(int)state++].Execute(data, _gameConfig);
            }

            return new StateResultData(previousState, result.MovementValue);
        }
    }
}