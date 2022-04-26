using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    public abstract class PlacerBase : MonoBehaviour
    {
        protected const float CellSize = 1f;
        
        [SerializeField] protected Vector2Int _startingCellPosition;
        
        private Vector2 _previousCellPosition;

        protected virtual void OnEnable()
        {
            _previousCellPosition = _startingCellPosition;
        }

        protected virtual void Update()
        {
            if (_previousCellPosition != _startingCellPosition)
            {
                _previousCellPosition = _startingCellPosition;
                transform.localPosition = new Vector2(_startingCellPosition.x * CellSize, _startingCellPosition.y * CellSize);
            }
        }

        public virtual void Recreate()
        {
            _previousCellPosition = new Vector2Int();
        }
    }
}