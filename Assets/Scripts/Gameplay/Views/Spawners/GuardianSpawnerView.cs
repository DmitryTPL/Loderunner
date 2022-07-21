using Loderunner.Service;

namespace Loderunner.Gameplay
{
    public class GuardianSpawnerView : View<GuardianSpawnerPresenter>
    {
        protected override void PresenterAttached()
        {
            _presenter.SetSpawnPoint(transform);
        }
    }
}