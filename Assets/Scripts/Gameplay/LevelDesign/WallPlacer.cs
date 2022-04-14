using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    public class WallPlacer : PlacerBase
    {
        [Header("Blocks")] [SerializeField, Range(0, 23)]
        private int _blocksCount;

        [SerializeField] private WallBlockView _prefab;
        [SerializeField] private List<WallBlockView> _blocks = new();

        [Space, Header("Fall")] [SerializeField]
        private bool _canFallFromLeft;

        [SerializeField] private bool _canFallFromRight;
        [SerializeField] private BoxCollider2D _leftFallingPointCollider;
        [SerializeField] private BoxCollider2D _rightFallingPointCollider;
        [SerializeField] private BoxCollider2D _mainCollider;

        [Space, Header("Border")] [SerializeField]
        private bool _hasBorderForLeftMovement;

        [SerializeField] private bool _hasBorderForRightMovement;
        [SerializeField] private BoxCollider2D _borderForLeftMovementCollider;
        [SerializeField] private BoxCollider2D _borderForRightMovementCollider;

        [Space, Header("Floor")] [SerializeField]
        private Transform _floorTop;

        [SerializeField] private FloorView _floorView;

        private int _previousBlocksCount;
        private bool _previousCanFallLeft;
        private bool _previousCanFallRight;
        private bool _previousHasBorderForLeftMovement;
        private bool _previousHasBorderForRightMovement;

        protected override void OnEnable()
        {
            base.OnEnable();

            _previousBlocksCount = _blocksCount;
            _previousCanFallLeft = _canFallFromLeft;
            _previousCanFallRight = _canFallFromRight;
            _previousHasBorderForLeftMovement = _hasBorderForLeftMovement;
            _previousHasBorderForRightMovement = _hasBorderForRightMovement;
        }

        protected override void Update()
        {
            base.Update();

            TryChangeBlocks();
            TryChangeFallEdges();
            TryChangeBorders();
        }

        [ContextMenu("Recreate blocks")]
        public void RecreateBlocks()
        {
            var blocks = _blocksCount;

            _blocksCount = 0;

            Update();

            _blocksCount = blocks;

            Update();
        }

        private void TryChangeFallEdges()
        {
            if (_previousCanFallLeft != _canFallFromLeft || _previousCanFallRight != _canFallFromRight)
            {
                RecalculateFallEdges();

                _previousCanFallLeft = _canFallFromLeft;
                _previousCanFallRight = _canFallFromRight;
            }
        }

        private void TryChangeBlocks()
        {
            if (_previousBlocksCount == _blocksCount)
            {
                return;
            }

            if (_blocks.Count > _blocksCount)
            {
                for (var i = _blocksCount; i < _blocks.Count; i++)
                {
                    DestroyImmediate(_blocks[i].gameObject);
                }

                _blocks.RemoveRange(_blocksCount, _blocks.Count - _blocksCount);
            }
            else
            {
                var blocksCount = _blocks.Count;

                for (var i = blocksCount; i < _blocksCount; i++)
                {
                    var block = PrefabUtility.InstantiatePrefab(_prefab) as WallBlockView;

                    block.transform.position = new Vector3(transform.position.x + i * CellSize, transform.position.y);
                    block.transform.parent = transform;

                    _blocks.Add(block);
                }
            }

            _floorTop.position = new Vector2(transform.position.x, transform.position.y + CellSize);

            _previousBlocksCount = _blocksCount;

            _floorView.WallBlocks = _blocks;

            RecalculateFallEdges();
            RecalculateBorders();
        }

        private void RecalculateFallEdges()
        {
            _mainCollider.enabled = _blocks.Count >= 0;
            _leftFallingPointCollider.enabled = _blocks.Count >= 0 && _canFallFromLeft;
            _rightFallingPointCollider.enabled = _blocks.Count >= 0 && _canFallFromRight;

            if (_blocks.Count == 0)
            {
                return;
            }

            var mainColliderWidth = CellSize * _blocks.Count;
            var mainColliderOffset = mainColliderWidth / 2;

            if (_canFallFromLeft)
            {
                mainColliderWidth -= _leftFallingPointCollider.size.x;
                mainColliderOffset += _leftFallingPointCollider.size.x / 2;
                _leftFallingPointCollider.offset = new Vector2(_leftFallingPointCollider.size.x / 2, 
                    CellSize - _leftFallingPointCollider.size.y / 2);
            }

            if (_canFallFromRight)
            {
                mainColliderWidth -= _rightFallingPointCollider.size.x;
                mainColliderOffset -= _rightFallingPointCollider.size.x / 2;
                _rightFallingPointCollider.offset =
                    new Vector2(CellSize * _blocks.Count - _rightFallingPointCollider.size.x / 2,
                        CellSize - _rightFallingPointCollider.size.y / 2);
            }

            _leftFallingPointCollider.enabled = _canFallFromLeft;
            _rightFallingPointCollider.enabled = _canFallFromRight;

            _mainCollider.size = new Vector2(mainColliderWidth, _mainCollider.size.y);
            _mainCollider.offset = new Vector2(mainColliderOffset, CellSize - _mainCollider.size.y / 2);
        }

        private void TryChangeBorders()
        {
            _borderForRightMovementCollider.enabled = _hasBorderForRightMovement;

            if (_previousHasBorderForLeftMovement != _hasBorderForLeftMovement ||
                _previousHasBorderForRightMovement != _hasBorderForRightMovement)
            {
                RecalculateBorders();
            }
        }

        private void RecalculateBorders()
        {
            if (_blocks.Count == 0)
            {
                return;
            }

            var offset = CellSize / 4 + 0.02f;

            if (_hasBorderForLeftMovement)
            {
                _borderForLeftMovementCollider.size = new Vector2(CellSize / 2, CellSize - offset);
                _borderForLeftMovementCollider.offset = new Vector2(CellSize * _blocks.Count - _borderForLeftMovementCollider.size.x / 2,
                    _borderForLeftMovementCollider.size.y / 2 + offset / 2);
            }

            if (_hasBorderForRightMovement)
            {
                _borderForRightMovementCollider.size = new Vector2(CellSize / 2, CellSize - offset);
                _borderForRightMovementCollider.offset = new Vector2(_borderForRightMovementCollider.size.x / 2,
                    _borderForRightMovementCollider.size.y / 2 + offset / 2);
            }

            _borderForLeftMovementCollider.enabled = _hasBorderForLeftMovement;
            _borderForRightMovementCollider.enabled = _hasBorderForRightMovement;

            _previousHasBorderForLeftMovement = _hasBorderForLeftMovement;
            _previousHasBorderForRightMovement = _hasBorderForRightMovement;
        }
    }
}