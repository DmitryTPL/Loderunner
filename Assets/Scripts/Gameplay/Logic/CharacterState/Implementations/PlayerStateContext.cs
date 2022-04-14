namespace Loderunner.Gameplay
{
    public class PlayerStateContext : CharacterStateContext<PlayerStateData>
    {
        public PlayerStateContext(GameConfig gameConfig, PlayerConfig characterConfig, PlayerStateData stateData) 
            : base(stateData)
        {
            States = new()
            {
                { (int)CharacterState.Moving, new MoveState(gameConfig, characterConfig, stateData) },
                { (int)CharacterState.CrossbarCrawling, new CrossbarCrawlingState(gameConfig, characterConfig, stateData) },
                { (int)CharacterState.LadderClimbing, new LadderClimbingState(gameConfig, characterConfig, stateData) },
                { (int)CharacterState.Falling, new FallingState(gameConfig, characterConfig, stateData) },
                { (int)CharacterState.RemoveBlock, new RemoveBlockState(gameConfig, characterConfig, stateData) },
            };
        }
    }
}