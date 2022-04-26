using Loderunner.Gameplay;

namespace Loderunner.Install
{
    public class PlayerSpawnerPresenterFactory : FactoryBase<PlayerSpawnerPresenter>
    {
        private readonly IGameObjectCreator _creator;

        public PlayerSpawnerPresenterFactory(IGameObjectCreator creator)
        {
            _creator = creator;
        }
        
        protected override PlayerSpawnerPresenter CreateEntryWithDependencies()
        {
            return new PlayerSpawnerPresenter(_creator);
        }
    }
}