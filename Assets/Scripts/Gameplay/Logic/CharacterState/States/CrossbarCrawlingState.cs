namespace Loderunner.Gameplay
{
    public class CrossbarCrawlingState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            return new StateResult(true);
        }
    }
}