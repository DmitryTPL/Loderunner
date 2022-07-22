using Cysharp.Threading.Tasks;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class WallBlockPresenter : Presenter, IWallBlockWithBorders
    {
        private readonly AsyncReactiveProperty<bool> _isLeftBorderActive = new (false);
        private readonly AsyncReactiveProperty<bool> _isRightBorderActive = new (false);
        
        protected readonly GameConfig _gameConfig;
        protected Vector2 _position;
        
        public IReadOnlyAsyncReactiveProperty<bool> IsLeftBorderActive => _isLeftBorderActive;
        public IReadOnlyAsyncReactiveProperty<bool> IsRightBorderActive => _isRightBorderActive;

        protected WallBlockPresenter(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }
        
        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public bool IsCharacterInBorders(Vector2 characterPosition)
        {
            return _position.x <= characterPosition.x && _position.x + _gameConfig.CellSize > characterPosition.x;
        }
        
        public void ChangeLeftBorderActivity(bool isActive)
        {
            _isLeftBorderActive.Value = isActive;
        }
        
        public void ChangeRightBorderActivity(bool isActive)
        {
            _isRightBorderActive.Value = isActive;
        }
    }
}