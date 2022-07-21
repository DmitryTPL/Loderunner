using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelExitView : View<LevelExitPresenter>
    {
        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();

            if (character != null)
            {
                _presenter.PlayerReachedLevelExit();
            }
        }
    }
}