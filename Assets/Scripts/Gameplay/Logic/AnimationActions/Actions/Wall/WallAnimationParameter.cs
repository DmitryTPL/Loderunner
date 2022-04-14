using UnityEngine;

namespace Loderunner.Gameplay
{
    public static class WallAnimationParameter
    {
        public static readonly int IsRemoving = Animator.StringToHash("IsRemoving");
        public static readonly int IsRestoring = Animator.StringToHash("IsRestoring");
    }
}