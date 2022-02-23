namespace Loderunner.Gameplay
{
    public class FallingState : CharacterStateBase
    {
        public override StateResult Execute(StateInitialData data, GameConfig gameConfig)
        {
            return new StateResult(true);
        }
    }
}