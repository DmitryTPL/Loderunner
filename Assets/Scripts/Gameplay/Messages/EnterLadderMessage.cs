using UnityEngine;

namespace Loderunner.Gameplay
{
    public readonly struct EnterLadderMessage
    {
        public ICharacterView CharacterView { get; }
        public ClimbingData Data { get; }
        
        public EnterLadderMessage(ICharacterView characterView, Vector3 ladderBottomCenter, Vector3 ladderTop)
        {
            CharacterView = characterView;
            Data = new ClimbingData(ladderBottomCenter.y, ladderBottomCenter.x, ladderTop.y);
        }
    }
}