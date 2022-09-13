using UnityEngine;

namespace Loderunner.Gameplay
{
    public static class CharacterAnimationParameter
    {
        public static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public static readonly int IsClimbing = Animator.StringToHash("IsClimbing");
        public static readonly int IsCrawling = Animator.StringToHash("IsCrawling");
        public static readonly int ClimbFinished = Animator.StringToHash("ClimbFinished");
        public static readonly int CrawlFinished = Animator.StringToHash("CrawlFinished");
        public static readonly int IsFalling = Animator.StringToHash("IsFalling");
        public static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
        public static readonly int IsRemovingBlock = Animator.StringToHash("IsRemovingBlock");
        public static readonly int GuardianSpawn = Animator.StringToHash("Spawn");
        public static readonly int PlayerDied = Animator.StringToHash("Death");
    }
}