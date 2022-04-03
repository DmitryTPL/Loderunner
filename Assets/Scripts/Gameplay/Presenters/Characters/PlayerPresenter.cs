using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class PlayerPresenter : CharacterPresenter
    {
        private readonly PlayerStateData _playerStateData;

        public PlayerPresenter(PlayerStateContext playerStateContext, IAsyncEnumerableReceiver receiver, IFallPointHolder fallPointHolder,
            IFloorPointHolder floorPointHolder)
            : base(playerStateContext, receiver, fallPointHolder, floorPointHolder,
                playerStateContext.StateData)
        {
            _playerStateData = playerStateContext.StateData;
        }

        public override int CharacterId => 1;

        public void UpdatePlayerRemovingBlock(RemoveBlockType blockType)
        {
            _playerStateData.RemoveBlockType = blockType;
        }
    }
}