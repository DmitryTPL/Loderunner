using UnityEngine;

namespace Loderunner.Gameplay
{
    public class PlayerStateData : StateData
    {
        public RemoveBlockType RemoveBlockType { get; set;}
        public Vector2 RemoveBlockCharacterAlignedPosition { get; set; }
    }
}