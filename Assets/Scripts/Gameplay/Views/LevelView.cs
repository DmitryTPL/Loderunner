using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelView : View<LevelPresenter>
    {
        [SerializeField] private Transform _playerStartPosition;
        
        public Transform PlayerStartPosition => _playerStartPosition;
    }
}