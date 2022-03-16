using System;

namespace Loderunner.Gameplay
{
    public interface IFallPointHolder
    {
        float FallPoint { get; }
       
        bool IsGrounded { get; }
    }
}