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
        private readonly ILevelFinishedObserver _levelFinishedObserver;
        private readonly PlayerStateData _playerStateData;
        private bool _isPlayerRemovingBlock;

        public event Action<Vector2, RemoveBlockType> BlockRemoving;
        public event Action BlockRemoved;
        public event Action Cached;

        public override CharacterType CharacterType => CharacterType.Player;

        public PlayerPresenter(PlayerStateContext playerStateContext, IAsyncEnumerableReceiver receiver, IAsyncEnumerablePublisher publisher,
            ICharacterFallObserver characterFallObserver, IWallBlockRemover wallBlockRemover, GameConfig gameConfig,
            ILevelFinishedObserver levelFinishedObserver)
            : base(playerStateContext, receiver, publisher, characterFallObserver, playerStateContext.StateData)
        {
            _wallBlockRemover = wallBlockRemover;
            _gameConfig = gameConfig;
            _levelFinishedObserver = levelFinishedObserver;
            _playerStateData = playerStateContext.StateData;

            levelFinishedObserver.AddTo(DisposeCancellationToken);
            wallBlockRemover.AddTo(DisposeCancellationToken);

            receiver.Receive<PlayerCachedMessage>().Subscribe(OnCached).AddTo(DisposeCancellationToken);
        }

        public override void CharacterCreated(int id)
        {
            base.CharacterCreated(id);
            
            _wallBlockRemover.BindCharacter(id);
            _levelFinishedObserver.BindCharacter(id);
            
            _receiver.Receive<WallBlockRemovingBeganMessage>().Where(m => m.IsCharacterMatch(id)).Subscribe(OnWallBlockRemovingBegan)
                .AddTo(DisposeCancellationToken);
            _receiver.Receive<CharacterReachedGoldMessage>().Where(m => m.IsCharacterMatch(id)).Subscribe(OnPlayerReachedGold).AddTo(DisposeCancellationToken);
            _receiver.Receive<PlayerReachedLevelExitMessage>().Subscribe(OnPlayerReachedLevelExit).AddTo(DisposeCancellationToken);
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

                RemoveWall(blockType, playerPosition).Forget();
            }
        }

        private async UniTask RemoveWall(RemoveBlockType blockType, Vector2 playerPosition)
        {
            await foreach (var state in _wallBlockRemover.RemoveBlock(blockType, playerPosition, Id).WithCancellation(DisposeCancellationToken))
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

                case CharacterState.Moving:
                case CharacterState.CrossbarCrawling:
                case CharacterState.LadderClimbing:
                case CharacterState.Falling:
                    _publisher.Publish(new PlayerMovedMessage(updatedStateData.NextCharacterPosition));
                    
                    base.ApplyState(updatedStateData);
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

        private void OnPlayerReachedGold(CharacterReachedGoldMessage message)
        {
            _publisher.Publish(new CharacterCollectGoldMessage(message.GoldGuid, Id));
        }

        private void OnPlayerReachedLevelExit(PlayerReachedLevelExitMessage _)
        {
            CanAct = false;
        }

        private void OnCached(PlayerCachedMessage _)
        {
            CanAct = false;
            Cached?.Invoke();
        }

        public void PlayerDeathFinished()
        {
            this.Log("Player Death Finished");
            
            _publisher.Publish(new PlayerDiedMessage());
        }
    }
}