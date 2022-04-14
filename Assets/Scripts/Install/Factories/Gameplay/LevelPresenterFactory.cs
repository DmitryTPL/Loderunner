using Loderunner.Gameplay;

namespace Loderunner.Install
{
    public class LevelPresenterFactory : FactoryBase<LevelPresenter>
    {
        protected override LevelPresenter CreateEntryWithDependencies()
        {
            return new LevelPresenter();
        }
    }
}