namespace Loderunner.Gameplay
{
    public interface IFallStateData
    {
        bool IsGrounded { get; set; }
        
        float FallPoint { get; set; }
    }
}