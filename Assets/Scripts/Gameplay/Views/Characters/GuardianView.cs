using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GuardianView : View<GuardianPresenter>, ICharacterInfo
    {
        [SerializeField] private AnimationHandler _animationHandler;
        
        public CharacterType CharacterType => CharacterType.Guardian;
        public int CharacterId { get; set; }
        public Vector2 Position => transform.position;
    }
}