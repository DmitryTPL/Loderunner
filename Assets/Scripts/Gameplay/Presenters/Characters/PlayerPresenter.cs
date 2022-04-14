using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public sealed class PlayerPresenter : CharacterPresenter
    {
        private readonly IWallBlockRemover _wallBlockRemover;
        private readonly GameConfig _gameConfig;
        private readonly PlayerStateData _playerStateData;
        private bool _isPlayerRemovingBlock;

        public event Action<Vector2, RemoveBlockType> BlockRemoving;
        public event Action BlockRemoved;

        public override int CharacterId
        {
            get => 1;
            set => throw new ArgumentException("Unable to set player Id");
        }

        public PlayerPresenter(PlayerStateContext playerStateContext, IAsyncEnumerableReceiver receiver, ICharacterFallObserver characterFallObserver,
             IWallBlockRemover wallBlockRemover, GameConfig gameConfig)
            : base(playerStateContext, receiver, characterFallObserver, playerStateContext.StateData)
        {
            _wallBlockRemover = wallBlockRemover;
            _gameConfig = gameConfig;
            _playerStateData = playerStateContext.StateData;

            (_wallBlockRemover as ICharacterFilter).CharacterId = CharacterId;
            
            receiver.Receive<WallBlockRemovingBeganMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnWallBlockRemovingBegan).AddTo(_unsubscribeTokenSource.Token);
        }

        public void UpdatePlayerRemovingBlock(RemoveBlockType blockType, Vector2 playerPosition)
        {
            if (blockType == RemoveBlockType.None)
            {
                return;
            }
            
            if (!_isPlayerRemovingBlock)
            {
                _isPlayerRemovingBlock = true;
                _playerStateData.RemoveBlockType = blockType;

                RemoveWall(blockType, playerPosition);
            }
        }

        private async UniTask RemoveWall(RemoveBlockType blockType, Vector2 playerPosition)
        {
            await foreach (var state in _wallBlockRemover.RemoveBlock(blockType, playerPosition, CharacterId))
            {
                switch (state)
                {
                    case WallBlockLifeState.None:
                    case WallBlockLifeState.Removed:
                        _isPlayerRemovingBlock = false;
                        _playerStateData.RemoveBlockCharacterAlignedPosition = Vector2.zero;
                        _playerStateData.RemoveBlockType = RemoveBlockType.None;
                        BlockRemoved?.Invoke();
                        break;
                }
            }
        }

        protected override void ApplyState(UpdatedStateData updatedStateData)
        {
            switch (updatedStateData.CurrentState)
            {
                case CharacterState.RemoveBlock:
                    BlockRemoving?.Invoke(updatedStateData.NextCharacterPosition, _playerStateData.RemoveBlockType);
                    break;
                default:
                    base.ApplyState(updatedStateData);
                    break;
            }
        }

        private void OnWallBlockRemovingBegan(WallBlockRemovingBeganMessage message)
        {
            var alignedPlayerPositionX = _playerStateData.RemoveBlockType switch
            {
                RemoveBlockType.Left => message.WallBlockPosition.x + _gameConfig.CellSize * 1.5f,
                RemoveBlockType.Right => message.WallBlockPosition.x - _gameConfig.CellSize * 0.5f,
                _ => _playerStateData.MovingData.CharacterPosition.x
            };

            _playerStateData.RemoveBlockCharacterAlignedPosition = 
                new Vector2(alignedPlayerPositionX, message.WallBlockPosition.y + _gameConfig.CellSize);
        }
    }
}