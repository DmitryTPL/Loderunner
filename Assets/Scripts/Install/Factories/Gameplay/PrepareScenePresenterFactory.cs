using Loderunner.Gameplay;

namespace Loderunner.Install
{
    public class PrepareScenePresenterFactory : FactoryBase<PrepareScenePresenter>
    {
        private readonly ILevelCreator _creator;

        public PrepareScenePresenterFactory(ILevelCreator creator)
        {
            _creator = creator;
        }

        protected override PrepareScenePresenter CreateEntryWithDependencies()
        {
            return new PrepareScenePresenter(_creator);
        }
    }
}