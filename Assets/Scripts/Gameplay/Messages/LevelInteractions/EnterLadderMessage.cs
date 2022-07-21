using UnityEngine;

namespace Loderunner.Gameplay
{
    public readonly struct EnterLadderMessage: IMessageForCharacter
    {
        public int CharacterId { get; }
        public ClimbingData Data { get; }
        
        public EnterLadderMessage(int characterId, Vector3 ladderBottomCenter, Vector3 ladderTop)
        {
            CharacterId = characterId;
            Data = new ClimbingData(ladderBottomCenter.y, ladderBottomCenter.x, ladderTop.y);
        }
    }

    
}