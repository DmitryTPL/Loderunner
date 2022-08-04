using UnityEngine;

namespace Loderunner.Gameplay
{
    public readonly struct GuardianDropGoldMessage
    {
        public Vector2Int GuardianPosition { get; }

        public GuardianDropGoldMessage(Vector2Int guardianPosition)
        {
            GuardianPosition = guardianPosition;
        }
    }
}