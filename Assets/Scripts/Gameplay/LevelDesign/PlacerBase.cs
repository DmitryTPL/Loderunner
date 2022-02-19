using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    public abstract class PlacerBase : MonoBehaviour
    {
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
                transform.localPosition = new Vector3(_startingCellPosition.x * GlobalConstant.CellValue, 
                    _startingCellPosition.y * GlobalConstant.CellValue, 0);
            }
        }
    }
}