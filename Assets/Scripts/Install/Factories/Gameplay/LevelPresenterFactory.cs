using Loderunner.Gameplay;

namespace Loderunner.Install
{
    public class LevelPresenterFactory : FactoryBase<LevelPresenter>
    {
        private readonly LevelData _levelData;

        public LevelPresenterFactory(LevelData levelData)
        {
            _levelData = levelData;
        }
        
        protected override LevelPresenter CreateEntryWithDependencies()
        {
            return new LevelPresenter(_levelData);
        }
    }
}