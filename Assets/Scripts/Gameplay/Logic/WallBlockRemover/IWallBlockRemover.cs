﻿using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IWallBlockRemover : IBoundToCharacter, IDisposable
    {
        IUniTaskAsyncEnumerable<WallBlockLifeState> RemoveBlock(RemoveBlockType removeBlockType, Vector2 characterPosition, int removerId);
    }
}