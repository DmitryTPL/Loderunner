namespace Loderunner.Gameplay
{
    public class CrossbarCrawlingIdleState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            return new StateResult(true);
        }
    }
}