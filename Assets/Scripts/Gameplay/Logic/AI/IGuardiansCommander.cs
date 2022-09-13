using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IGuardiansCommander: IDisposable
    {
        void Register(int id);
        UniTask<PathResult> GetPath(int id, Vector2Int mapPosition);
    }
}