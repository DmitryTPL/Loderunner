using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class FloorPresenter : Presenter, IWallBlocksHolder
    {
        private readonly IAsyncEnumerablePublisher _publisher;
        private readonly GameConfig _gameConfig;
        private readonly List<IRemovableWallBlock> _wallBlocks = new();

        private float _leftFallPoint;
        private float _rightFallPoint;
        private Vector2 _floorStartPosition;

        public FloorPresenter(IAsyncEnumerablePublisher publisher, GameConfig gameConfig)
        {
            _publisher = publisher;
            _gameConfig = gameConfig;
        }

        public void SetFloorStartPosition(Vector2 position)
        {
            _floorStartPosition = position;
        }

        public void AddWallBlockPresenter(IRemovableWallBlock wallBlockPresenter)
        {
            _wallBlocks.Add(wallBlockPresenter);
        }

        public void FloorReached(ICharacterInfo character, int floorId, float floorTop)
        {
            _publisher.Publish(new FloorReachedMessage(character.CharacterId, floorId, floorTop, this));
        }

        public void GotOffTheFloor(ICharacterInfo character, int floorId)
        {
            _publisher.Publish(new GotOffTheFloorMessage(character.CharacterId, floorId, this));
        }

        public bool CanRemoveBlock(RemoveBlockType removeBlockType, Vector2 characterPosition)
        {
            return GetWallBlockToRemove(removeBlockType, characterPosition) != null;
        }

        public IUniTaskAsyncEnumerable<WallBlockLifeState> RemoveBlock(RemoveBlockType removeBlockType, Vector2 characterPosition, int removerId)
        {
            return UniTaskAsyncEnumerable.Create<WallBlockLifeState>(async (writer, token) =>
            {
                var wallBlock = GetWallBlockToRemove(removeBlockType, characterPosition);

                if (wallBlock == null)
                {
                    await writer.YieldAsync(WallBlockLifeState.None);
                    return;
                }

                await foreach (var state in wallBlock.TryRemove(removerId).WithCancellation(token))
                {
                    if (token.IsCancellationRequested)
                    {
                        await writer.YieldAsync(WallBlockLifeState.None);
                        return;
                    }
                    
                    await writer.YieldAsync(state);
                }
            });
        }

        private IRemovableWallBlock GetWallBlockToRemove(RemoveBlockType removeBlockType, Vector2 characterPosition)
        {
            for (var i = 0; i < _wallBlocks.Count; i++)
            {
                var wallBlock = _wallBlocks[i];

                if (!wallBlock.IsCharacterInBorders(characterPosition))
                {
                    continue;
                }

                switch (removeBlockType)
                {
                    case RemoveBlockType.Left:
                        return i == 0 ? null : _wallBlocks[i - 1];
                    case RemoveBlockType.Right:
                        return i == _wallBlocks.Count - 1 ? null : _wallBlocks[i + 1];
                    default:
                        throw new ArgumentOutOfRangeException(nameof(removeBlockType), removeBlockType, null);
                }
            }

            // character is standing not on a wall block
            switch (removeBlockType)
            {
                case RemoveBlockType.Left:
                    if (characterPosition.x > _floorStartPosition.x + _gameConfig.CellSize * _wallBlocks.Count &&
                        characterPosition.x < _floorStartPosition.x + _gameConfig.CellSize * (_wallBlocks.Count + 1))
                    {
                        return _wallBlocks[^1];
                    }

                    break;
                case RemoveBlockType.Right:
                    if (characterPosition.x < _floorStartPosition.x &&
                        characterPosition.x > _floorStartPosition.x - _gameConfig.CellSize)
                    {
                        return _wallBlocks[0];
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(removeBlockType), removeBlockType, null);
            }

            return null;
        }
    }
}