using Loderunner.Service;

namespace Loderunner.Gameplay
{
    public class PrepareScenePresenter : Presenter
    {
        private readonly ILevelCreator _levelCreator;

        public PrepareScenePresenter(ILevelCreator levelCreator)
        {
            _levelCreator = levelCreator;
        }

        public void CreateLevel(int levelIndex)
        {
            _levelCreator.CreateLevel(levelIndex);
        }
    }
}