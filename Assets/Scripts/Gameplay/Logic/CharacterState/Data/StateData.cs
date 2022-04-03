namespace Loderunner.Gameplay
{
    public class StateData : IFallStateData, IFloorData
    {
        public MovingData MovingData { get; set; }
        public ClimbingData ClimbingData { get; set;}
        public CrawlingData CrawlingData { get; set;}
        public CharacterState PreviousState { get; set;}
        public BorderType BorderReachedType { get; set;}
        public bool IsGrounded { get; set;}
        public float FallPoint { get; set;}
        public float FloorPoint { get; set;}
    }
}