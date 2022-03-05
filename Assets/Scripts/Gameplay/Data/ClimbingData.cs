namespace Loderunner.Gameplay
{
    public struct ClimbingData
    {
        public float Bottom { get; }
        public float Center { get; }
        public float Top { get; }

        public bool IsEmpty => Bottom == 0 && Top == 0;

        public ClimbingData(float bottom, float center, float top)
        {
            Bottom = bottom;
            Center = center;
            Top = top;
        }
    }
}