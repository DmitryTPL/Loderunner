using System;
using UnityEngine;

public static class Vector2Extension
{
    public static Vector2Int ToVector2Int(this Vector2 origin)
    {
        return new Vector2Int((int)Math.Floor(origin.x), (int)Math.Floor(origin.y));
    }
}