using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class WallBlockRemover : IDisposable, ICharacterFilter, IWallBlockRemover
    {
        private readonly CancellationTokenSource _unsubscribeTokenSource = new();
        private readonly LinkedList<IWallBlocksHolder> _wallBlocksHolders = new();

        public int Id { get; set; }

        public WallBlockRemover(IAsyncEnumerableReceiver receiver)
        {
            receiver.Receive<GotOffTheFloorMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(GotOffTheFloor).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<FloorReachedMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnFloorReached).AddTo(_unsubscribeTokenSource.Token);
        }

        public void Dispose()
        {
            _unsubscribeTokenSource.Dispose();
        }

        public IUniTaskAsyncEnumerable<WallBlockLifeState> RemoveBlock(RemoveBlockType removeBlockType, Vector2 characterPosition, int removerId)
        {
            return UniTaskAsyncEnumerable.Create<WallBlockLifeState>(async (writer, token) =>
            {
                IWallBlocksHolder wallBlocksHolder = null;
                
                foreach (var holder in _wallBlocksHolders)
                {
                    if (holder.CanRemoveBlock(removeBlockType, characterPosition))
                    {
                        wallBlocksHolder = holder;
                        break;
                    }
                }

                if (wallBlocksHolder == null)
                {
                    await writer.YieldAsync(WallBlockLifeState.None);
                    return;
                }

                await foreach (var state in wallBlocksHolder.RemoveBlock(removeBlockType, characterPosition, removerId).WithCancellation(token))
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

        private void OnFloorReached(FloorReachedMessage message)
        {
            _wallBlocksHolders.AddLast(message.WallBlocksHolder);
        }

        private void GotOffTheFloor(GotOffTheFloorMessage message)
        {
            _wallBlocksHolders.Remove(message.WallBlocksHolder);
        }
    }
}