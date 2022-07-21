using Loderunner.Gameplay;
using UnityEngine;

public static class Collider2DExtension
{
    public static ICharacterInfo TryGetCharacter(this Collider2D collider)
    {
        return collider.GetComponentInChildren<ICharacterInfo>();
    }
}