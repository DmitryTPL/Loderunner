using Loderunner.Gameplay;

namespace Loderunner.Install
{
    public class PrepareScenePresenterFactory : FactoryBase<PrepareScenePresenter>
    {
        private readonly IGameObjectCreator _objectCreator;

        public PrepareScenePresenterFactory(IGameObjectCreator objectCreator)
        {
            _objectCreator = objectCreator;
        }

        protected override PrepareScenePresenter CreateEntryWithDependencies()
        {
            return new PrepareScenePresenter(_objectCreator);
        }
    }
}