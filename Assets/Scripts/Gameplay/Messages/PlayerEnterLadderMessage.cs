using UnityEngine;

namespace Loderunner.Gameplay
{
    public struct PlayerEnterLadderMessage
    {
        public ClimbingData Data { get; }
        
        public PlayerEnterLadderMessage(Vector3 ladderBottomCenter, Vector3 ladderTop)
        {
            Data = new ClimbingData(ladderBottomCenter.y, ladderBottomCenter.x, ladderTop.y);
        }
    }
}