namespace Loderunner.Gameplay
{
    public struct ClimbingData
    {
        public float LadderBottom { get; }
        public float LadderCenter { get; }
        public float LadderTop { get; }

        public bool IsEmpty => LadderBottom == 0 && LadderTop == 0;

        public ClimbingData(float ladderBottom, float ladderCenter, float ladderTop)
        {
            LadderBottom = ladderBottom;
            LadderCenter = ladderCenter;
            LadderTop = ladderTop;
        }
    }
}