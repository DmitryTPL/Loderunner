namespace Loderunner.Gameplay
{
    public class RemoveBlockState : CharacterStateBase<PlayerStateData>
    {
        public RemoveBlockState(GameConfig gameConfig, ICharacterConfig characterConfig, PlayerStateData data)
            : base(gameConfig, characterConfig, data)
        {
        }

        public override StateResult Execute()
        {
            if (_data.RemoveBlockType == RemoveBlockType.None
                || !_data.IsGrounded
                || !_data.ClimbingData.IsEmpty
                || !_data.CrawlingData.IsEmpty)
            {
                return new StateResult(true);
            }
            
            return new StateResult(true);
        }
    }
}