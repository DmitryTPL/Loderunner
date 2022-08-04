using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GoldSpawnerView : View<GoldSpawnerPresenter>
    {
        [SerializeField] private Transform _goldSpawnRoot;
        
        protected override void PresenterAttached()
        {
            base.PresenterAttached();
            
            _presenter.SetGoldSpawnRoot(_goldSpawnRoot);
        }
    }
}