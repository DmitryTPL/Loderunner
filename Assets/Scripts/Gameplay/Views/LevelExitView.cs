using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelExitView : View<LevelExitPresenter>
    {
        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var characterView = otherCollider.gameObject.GetComponent<ICharacterInfo>();

            if (characterView != null)
            {
                _presenter.PlayerReachedLevelExit();
            }
        }
    }
}