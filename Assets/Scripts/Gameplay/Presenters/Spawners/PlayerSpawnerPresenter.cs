using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class PlayerSpawnerPresenter : Presenter
    {
        private readonly IPlayerCreator _playerCreator;

        public PlayerSpawnerPresenter(IPlayerCreator playerCreator)
        {
            _playerCreator = playerCreator;
        }

        public void SpawnPlayer(Transform spawnPoint)
        {
            _playerCreator.CreatePlayer(spawnPoint);
        }
    }
}