using System;

namespace Loderunner.Gameplay
{
    public interface ICharacterFallObserver: IDisposable {
        void BeginToFallFromCrossbar(float characterPositionY);

        void UpdateFallData(IFallStateData data);
    }
}