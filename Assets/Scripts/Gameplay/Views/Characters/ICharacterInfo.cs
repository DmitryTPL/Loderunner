using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface ICharacterInfo
    {
        CharacterType CharacterType { get; }
        int CharacterId { get; }
        Vector2 Position { get; }
    }
}