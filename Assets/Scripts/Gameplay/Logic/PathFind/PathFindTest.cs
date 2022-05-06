using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class PathFindTest : MonoBehaviour
    {
        [SerializeField] private Matrix<int> _map;
        [SerializeField] private Vector2Int _startPoint;
        [SerializeField] private Vector2Int _endPoint;
        
        [ContextMenu("Test path")]
        public void Test()
        {
            var pathFinder = new AStarPathFinder();

            var path = pathFinder.GetPath(_map, _startPoint, _endPoint);
        }
    }
}