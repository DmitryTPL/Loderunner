using Loderunner.Service;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Loderunner.Gameplay
{
    public class PrepareSceneView : View<PrepareScenePresenter>
    {
        [SerializeField] private Transform _playerStartPosition;
        
        [Inject] private PlayerView _playerPrefab;

        private void Start()
        {
            _resolver.Instantiate(_playerPrefab, _playerStartPosition.position, Quaternion.identity);
        }
    }
}