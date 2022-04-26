using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelView : View<LevelPresenter>
    {
        [SerializeField] private BoxCollider2D _boundsCollider;

        private void Start()
        {
            _presenter.SetCameraBounds(_boundsCollider.bounds);
        }
    }
}