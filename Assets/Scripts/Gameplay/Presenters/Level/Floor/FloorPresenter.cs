using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<int, IWallBlockWithBorders> _wallBlocks = new();

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

        public void AddWallBlock(IWallBlockWithBorders wallBlock, int index)
        {
            _wallBlocks.Add(index, wallBlock);
        }

        public void FloorReached(ICharacterInfo character, float floorTop)
        {
            _publisher.Publish(new FloorReachedMessage(character.CharacterId, Guid, floorTop, this));
        }

        public void GotOffTheFloor(ICharacterInfo character)
        {
            _publisher.Publish(new GotOffTheFloorMessage(character.CharacterId, Guid, this));
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

                ChangeNeighboursBordersActivity(wallBlock, true);

                await foreach (var state in wallBlock.TryRemove(removerId).WithCancellation(token))
                {
                    if (token.IsCancellationRequested)
                    {
                        await writer.YieldAsync(WallBlockLifeState.None);
                        return;
                    }
                    
                    await writer.YieldAsync(state);
                }
                
                ChangeNeighboursBordersActivity(wallBlock, false);
            });
        }

        private void ChangeNeighboursBordersActivity(IRemovableWallBlock removableWallBlock, bool isActive)
        {
            var wallBlockIndex = _wallBlocks.Where(w => w.Value == removableWallBlock).Select(w => w.Key).First();

            var minIndex = _wallBlocks.Keys.Min();
            var maxIndex = _wallBlocks.Keys.Max();
            
            if (wallBlockIndex >= minIndex && wallBlockIndex < maxIndex)
            {
                _wallBlocks[wallBlockIndex + 1].ChangeLeftBorderActivity(isActive);
            } 
            
            if (wallBlockIndex > minIndex && wallBlockIndex <= maxIndex)
            {
                _wallBlocks[wallBlockIndex - 1].ChangeRightBorderActivity(isActive); 
            }
        }

        private IRemovableWallBlock GetWallBlockToRemove(RemoveBlockType removeBlockType, Vector2 characterPosition)
        {
            foreach (var wallBlockKeyValue in _wallBlocks)
            {
                var wallBlockIndex = wallBlockKeyValue.Key;
                var wallBlock = wallBlockKeyValue.Value;
                
                if (!wallBlock.IsCharacterInBorders(characterPosition))
                {
                    continue;
                }

                switch (removeBlockType)
                {
                    case RemoveBlockType.Left:
                        return wallBlockIndex == _wallBlocks.Keys.Min() ? null : _wallBlocks[wallBlockIndex - 1] as IRemovableWallBlock;
                    case RemoveBlockType.Right:
                        return wallBlockIndex == _wallBlocks.Keys.Max() ? null : _wallBlocks[wallBlockIndex + 1] as IRemovableWallBlock;
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
                        return _wallBlocks[_wallBlocks.Keys.Max()] as IRemovableWallBlock;
                    }

                    break;
                case RemoveBlockType.Right:
                    if (characterPosition.x < _floorStartPosition.x &&
                        characterPosition.x > _floorStartPosition.x - _gameConfig.CellSize)
                    {
                        return _wallBlocks[_wallBlocks.Keys.Min()] as IRemovableWallBlock;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(removeBlockType), removeBlockType, null);
            }

            return null;
        }
    }
}