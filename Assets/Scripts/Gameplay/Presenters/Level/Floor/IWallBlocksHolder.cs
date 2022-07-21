using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IWallBlocksHolder
    {
        bool CanRemoveBlock(RemoveBlockType removeBlockType, Vector2 characterPosition);
        IUniTaskAsyncEnumerable<WallBlockLifeState> RemoveBlock(RemoveBlockType removeBlockType, Vector2 characterPosition, int removerId);
    }
}