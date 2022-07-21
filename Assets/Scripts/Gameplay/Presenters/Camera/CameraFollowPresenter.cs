using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class CameraFollowPresenter : Presenter
    {
        private float _orthographicSize;
        private float _aspect;
        private readonly GameConfig _gameConfig;
        private readonly ILevelData _levelData;
        private Vector2 _playerPosition;

        public CameraFollowPresenter(IAsyncEnumerableReceiver receiver, GameConfig gameConfig, ILevelData levelData)
        {
            _gameConfig = gameConfig;
            _levelData = levelData;
            
            receiver.Receive<PlayerMovedMessage>().Subscribe(OnPlayerMoved).AddTo(DisposeCancellationToken);
        }

        public void SetOrthographicSize(float orthographicSize, float aspect)
        {
            _orthographicSize = orthographicSize;
            _aspect = aspect;
        }

        public Vector3 GetNewCameraPosition(Vector3 transformPosition)
        {
            var halfWidth = _orthographicSize * _aspect;
            
            var nextCameraPosition = new Vector3(Mathf.Clamp(_playerPosition.x, _levelData.CameraBounds.min.x + halfWidth, _levelData.CameraBounds.max.x - halfWidth),
                Mathf.Clamp(_playerPosition.y, _levelData.CameraBounds.min.y + _orthographicSize, _levelData.CameraBounds.max.y - _orthographicSize), -1);

            return Vector3.Lerp(transformPosition, nextCameraPosition, _gameConfig.SmoothPlayerCameraMovementRatio);
        }

        private void OnPlayerMoved(PlayerMovedMessage message)
        {
            _playerPosition = message.Position;
        }
    }
}