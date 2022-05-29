using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IPathFinder
    {
        UniTask<PathResult> GetPath(Matrix<int> map, Vector2Int startPoint, Vector2Int goal);
    }
}