using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IGuardiansCommander
    {
        void Register(int id);
        UniTask<PathResult> GetPath(int id, Vector2Int mapPosition);
    }
}