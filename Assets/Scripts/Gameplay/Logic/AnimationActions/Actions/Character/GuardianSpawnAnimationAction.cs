namespace Loderunner.Gameplay
{
    public class GuardianSpawnAnimationAction : AnimationActionWithFinishAwaiting
    {
        protected override int Trigger => CharacterAnimationParameter.GuardianSpawn;
    }
}