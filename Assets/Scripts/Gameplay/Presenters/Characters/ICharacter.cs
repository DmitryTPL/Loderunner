using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface ICharacter
    {
        int Id { get; }
        Vector2 Position { get; }
        CharacterType CharacterType { get; }
    }
}