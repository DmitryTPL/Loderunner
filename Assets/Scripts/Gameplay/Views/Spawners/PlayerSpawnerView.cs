using Loderunner.Service;

namespace Loderunner.Gameplay
{
    public class PlayerSpawnerView : View<PlayerSpawnerPresenter>
    {
        public void Start()
        {
            _presenter.SpawnPlayer(transform);
        }
    }
}