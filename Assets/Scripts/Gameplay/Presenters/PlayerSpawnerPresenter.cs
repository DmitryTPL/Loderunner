using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class PlayerSpawnerPresenter : Presenter
    {
        private readonly IGameObjectCreator _creator;

        public PlayerSpawnerPresenter(IGameObjectCreator creator)
        {
            _creator = creator;
        }

        public void SpawnPlayer(Transform spawnPoint)
        {
            _creator.CreatePlayer(spawnPoint);
        }
    }
}