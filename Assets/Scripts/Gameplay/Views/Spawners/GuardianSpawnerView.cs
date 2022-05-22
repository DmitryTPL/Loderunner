using Loderunner.Service;

namespace Loderunner.Gameplay
{
    public class GuardianSpawnerView : View<GuardianSpawnerPresenter>
    {
        protected override void PresenterAttached()
        {
            _presenter.NeedToSpawnGuardian += OnNeedToSpawnGuardian;
        }

        private void OnDestroy()
        {
            _presenter.NeedToSpawnGuardian -= OnNeedToSpawnGuardian;
        }

        private void OnNeedToSpawnGuardian()
        {
            _presenter.Spawn(transform);
        }
    }
}