namespace Loderunner.Gameplay
{
    public class GuardianStateContext : CharacterStateContext<StateData>
    {
        public GuardianStateContext(GameConfig gameConfig, GuardianConfig characterConfig, StateData stateData) 
            : base(gameConfig, characterConfig, stateData)
        {
        }
    }
}