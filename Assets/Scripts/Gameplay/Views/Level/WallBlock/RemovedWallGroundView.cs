using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class RemovedWallGroundView : View<RemovedWallGroundPresenter>
    {
        [SerializeField] private BoxCollider2D _groundCollider;
        [SerializeField] private Transform _top;

        protected override void PresenterAttached()
        {
            base.PresenterAttached();
            
            _presenter.IsActive.ForEachAsync(OnActivityChanged).Forget();
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();
            
            if (character != null)
            {
                if (character.CharacterType != CharacterType.Guardian)
                {
                    return;
                }
                
                _presenter.GroundReached(character.CharacterId, _top.position.y);
            }
        }
        
        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();
            
            if (character != null)
            {
                if (character.CharacterType != CharacterType.Guardian)
                {
                    return;
                }
                
                _presenter.GotOffTheGround(character.CharacterId);
            }
        }

        private void OnActivityChanged(bool isActive)
        {
            _groundCollider.enabled = isActive;
        }
    }
}