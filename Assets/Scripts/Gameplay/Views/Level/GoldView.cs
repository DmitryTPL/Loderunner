using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GoldView : View<GoldPresenter>
    {
        private void Start()
        {
            _presenter.GoldHasBeenTaken += OnGoldHasBeenTaken;
        }

        private void OnDestroy()
        {
            _presenter.GoldHasBeenTaken -= OnGoldHasBeenTaken;
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();

            if (character != null)
            {
                _presenter.CharacterReachedGold(character);
            }
        }

        private void OnGoldHasBeenTaken()
        {
            Destroy(gameObject);
        }
    }
}