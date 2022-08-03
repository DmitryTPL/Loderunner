using UnityEngine;

namespace Loderunner.Gameplay
{
    public readonly struct GuardianDropGoldMessage
    {
        public Vector2Int DropPosition { get; }

        public GuardianDropGoldMessage(Vector2Int dropPosition)
        {
            DropPosition = dropPosition;
        }
    }
}