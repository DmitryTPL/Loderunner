using Loderunner.Service;

namespace Loderunner.Gameplay
{
    public class PrepareScenePresenter : Presenter
    {
        private readonly IGameObjectCreator _objectCreator;

        public PrepareScenePresenter(IGameObjectCreator objectCreator)
        {
            _objectCreator = objectCreator;
        }

        public void CreateLevel(int levelIndex)
        {
            _objectCreator.CreateLevel(levelIndex);
        }
    }
}