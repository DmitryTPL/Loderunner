using System;
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
            var characterView = otherCollider.gameObject.GetComponent<ICharacterInfo>();

            if (characterView != null)
            {
                _presenter.CharacterReachedGold(characterView);
            }
        }

        private void OnGoldHasBeenTaken()
        {
            Destroy(gameObject);
        }
    }
}