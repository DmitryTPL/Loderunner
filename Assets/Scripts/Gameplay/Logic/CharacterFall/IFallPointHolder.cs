using System;

namespace Loderunner.Gameplay
{
    public interface IFallPointHolder {
        void BeginToFallFromCrossbar(float characterPositionY);

        void UpdateFallData(IFallStateData data);
    }
}