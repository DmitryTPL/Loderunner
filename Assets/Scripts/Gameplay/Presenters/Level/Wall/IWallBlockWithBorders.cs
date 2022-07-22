using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IWallBlockWithBorders
    {
        bool IsCharacterInBorders(Vector2 characterPosition);
        
        void ChangeLeftBorderActivity(bool isActive);
        void ChangeRightBorderActivity(bool isActive);
    }
}