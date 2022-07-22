using Cysharp.Threading.Tasks;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class RemovedWallPresenter : Presenter
    {
        private readonly GameConfig _gameConfig;
        private readonly IAsyncEnumerablePublisher _publisher;
        private readonly AsyncReactiveProperty<bool> _isActive = new (false);

        private Vector2 _position;
        private RemovedWallGroundPresenter _removedWallGroundPresenter;

        public IReadOnlyAsyncReactiveProperty<bool> IsActive => _isActive;

        public RemovedWallPresenter(GameConfig gameConfig, IAsyncEnumerablePublisher publisher)
        {
            _gameConfig = gameConfig;
            _publisher = publisher;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public void SetRemovedWallGroundPresenter(RemovedWallGroundPresenter presenter)
        {
            _removedWallGroundPresenter = presenter;
        }

        public void ChangeActivity(bool isActive)
        {
            _isActive.Value = isActive;
            _removedWallGroundPresenter.ChangeActivity(isActive);
        }

        public void UpdateCharacterPosition(Vector2 characterPosition, int characterId)
        {
            var center = _position.x + _gameConfig.CellSize / 2;
            var fallBoundsValue = _gameConfig.CellSize / 8;
            
            if (characterPosition.x > center - fallBoundsValue && characterPosition.x < center + fallBoundsValue)
            {
                _publisher.Publish(new CharacterNeedToFallInRemovedBlockMessage(characterId, center));
                ChangeActivity(false);
            }
        }
    }
}