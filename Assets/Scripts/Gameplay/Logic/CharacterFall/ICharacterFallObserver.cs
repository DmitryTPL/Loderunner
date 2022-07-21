using System;

namespace Loderunner.Gameplay
{
    public interface ICharacterFallObserver : IBoundToCharacter, IDisposable
    {
        public bool IsGrounded { get; }
        
        void BeginToFallFromCrossbar(float characterPositionY);

        void UpdateFallData(IFallStateData data);
    }
}