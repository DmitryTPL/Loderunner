using Loderunner.Service;

namespace Loderunner.Gameplay
{
    public class PrepareSceneView : View<PrepareScenePresenter>
    {
        private void Start()
        {
            _presenter.CreateLevel(1);
        }
    }
}