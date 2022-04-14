using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IRemovableWallBlock
    {
        IUniTaskAsyncEnumerable<WallBlockLifeState> TryRemove(int removerId);

        bool IsCharacterInBorders(Vector2 characterPosition);
    }
}