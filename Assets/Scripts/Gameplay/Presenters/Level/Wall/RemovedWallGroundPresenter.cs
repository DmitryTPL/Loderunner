using Cysharp.Threading.Tasks;
using Loderunner.Service;

namespace Loderunner.Gameplay
{
    public class RemovedWallGroundPresenter : Presenter
    {
        private readonly AsyncReactiveProperty<bool> _isActive = new (false);
        
        public IReadOnlyAsyncReactiveProperty<bool> IsActive => _isActive;

        public void ChangeActivity(bool isActive)
        {
            _isActive.Value = isActive;
        }
    }
}