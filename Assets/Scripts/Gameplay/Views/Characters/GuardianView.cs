using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GuardianView : CharacterView<GuardianPresenter>
    {
        public override CharacterType CharacterType => CharacterType.Guardian;

        private void FixedUpdate()
        {
            var (horizontalDirection, verticalDirection) = _presenter.GetDirection();
            
            _presenter.UpdateCharacterMoveData(new MovingData(horizontalDirection, verticalDirection, Position));
            _presenter.UpdateCharacterState();
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();

            if (character != null)
            {
                switch (character.CharacterType)
                {
                    case CharacterType.Player:
                        _presenter.PlayerCached();
                        break;
                }
            }
        }
    }
}