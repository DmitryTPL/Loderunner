namespace Loderunner.Gameplay
{
    public class PlayerDiedAnimationAction : AnimationActionWithFinishAwaiting
    {
        protected override int Trigger => CharacterAnimationParameter.PlayerDied;
    }
}