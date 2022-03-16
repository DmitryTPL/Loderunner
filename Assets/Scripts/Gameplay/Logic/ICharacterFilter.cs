using System;

namespace Loderunner.Gameplay
{
    public interface ICharacterFilter
    {
        Func<int, bool> CharacterFilter { get; set; }
    }
}