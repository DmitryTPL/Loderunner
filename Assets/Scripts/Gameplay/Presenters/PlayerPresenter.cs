using Loderunner.Service;

namespace Loderunner.Gameplay
{
    public class PlayerPresenter : Presenter
    {
        public PlayerConfig PlayerConfig { get; }

        public PlayerPresenter(PlayerConfig playerConfig)
        {
            PlayerConfig = playerConfig;
        }
    }
}