using Loderunner.Gameplay;

namespace Loderunner.Install
{
    public class PlayerSpawnerPresenterFactory : FactoryBase<PlayerSpawnerPresenter>
    {
        private readonly IPlayerCreator _creator;

        public PlayerSpawnerPresenterFactory(IPlayerCreator creator)
        {
            _creator = creator;
        }
        
        protected override PlayerSpawnerPresenter CreateEntryWithDependencies()
        {
            return new PlayerSpawnerPresenter(_creator);
        }
    }
}