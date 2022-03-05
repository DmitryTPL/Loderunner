using System;

namespace Loderunner.Gameplay
{
    public interface IFallPointHolder
    {
        float FallPoint { get; }
        Func<int, bool> CharacterFilter { get; set; }
        bool IsGrounded { get; }
    }
}