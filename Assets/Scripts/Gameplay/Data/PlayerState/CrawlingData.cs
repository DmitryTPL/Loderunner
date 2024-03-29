﻿namespace Loderunner.Gameplay
{
    public struct CrawlingData
    {
        public bool IsFinished { get; }
        public float Left { get; }
        public float Right { get; }
        public float Center { get; }

        public bool IsEmpty => Left == 0 && Right == 0 && !IsFinished;
        
        public CrawlingData(float left, float right, float center)
        {
            IsFinished = false;
            Left = left;
            Right = right;
            Center = center;
        }
        
        public CrawlingData(bool isFinished)
        {
            IsFinished = isFinished;
            Left = 0;
            Right = 0;
            Center = 0;
        }
    }
}