namespace Loderunner.Gameplay
{
    public class PlayerStateContext : CharacterStateContext<PlayerStateData>
    {
        public PlayerStateContext(GameConfig gameConfig, PlayerConfig characterConfig, PlayerStateData stateData)
            : base(gameConfig, characterConfig, stateData)
        {
            States.Add((int)CharacterState.RemoveBlock, new RemoveBlockState(gameConfig, characterConfig, stateData));
        }
    }
}