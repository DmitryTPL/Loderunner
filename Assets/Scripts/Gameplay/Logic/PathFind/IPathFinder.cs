using System.Collections.Generic;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IPathFinder
    {
        Stack<Vector2Int> GetPath(Matrix<int> map, Vector2Int startPoint, Vector2Int goal);
    }
}