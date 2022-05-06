using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    public class WallPlacer : PlacerBase
    {
        [Header("Blocks")] [SerializeField, Range(0, 100)]
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

        public int BlocksCount => _blocksCount;

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
        public override void Recreate()
        {
            base.Recreate();
            
            var blocks = _blocksCount;   
            
            _blocksCount = 0;
            
            TryChangeBlocks();

            _blocksCount = blocks;
            
            _previousBlocksCount = 0;
            _previousCanFallLeft = !_canFallFromLeft;
            _previousHasBorderForLeftMovement = !_hasBorderForLeftMovement;
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

                    block.transform.position = new Vector2(transform.position.x + i * CellSize, transform.position.y);
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

            var fallEdgeSize = new Vector2(CellSize / 4, CellSize / 2);

            if (_canFallFromLeft)
            {
                _leftFallingPointCollider.size = fallEdgeSize;

                mainColliderWidth -= fallEdgeSize.x;
                mainColliderOffset += fallEdgeSize.x / 2;

                _leftFallingPointCollider.offset = new Vector2(fallEdgeSize.x / 2, CellSize * 0.75f);
            }

            if (_canFallFromRight)
            {
                _rightFallingPointCollider.size = fallEdgeSize;

                mainColliderWidth -= fallEdgeSize.x;
                mainColliderOffset -= fallEdgeSize.x / 2;

                _rightFallingPointCollider.offset = new Vector2(CellSize * _blocks.Count - fallEdgeSize.x / 2, CellSize * 0.75f);
            }

            _leftFallingPointCollider.enabled = _canFallFromLeft;
            _rightFallingPointCollider.enabled = _canFallFromRight;

            _mainCollider.size = new Vector2(mainColliderWidth, CellSize / 2);
            _mainCollider.offset = new Vector2(mainColliderOffset, CellSize * 0.75f);
        }

        private void TryChangeBorders()
        {
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
            var borderSize = new Vector2(CellSize / 2, CellSize - offset);

            if (_hasBorderForLeftMovement)
            {
                _borderForLeftMovementCollider.size = borderSize;
                _borderForLeftMovementCollider.offset = new Vector2(CellSize * _blocks.Count - borderSize.x / 2, borderSize.y / 2 + offset / 2);
            }

            if (_hasBorderForRightMovement)
            {
                _borderForRightMovementCollider.size = borderSize;
                _borderForRightMovementCollider.offset = new Vector2(borderSize.x / 2, borderSize.y / 2 + offset / 2);
            }

            _borderForLeftMovementCollider.enabled = _hasBorderForLeftMovement;
            _borderForRightMovementCollider.enabled = _hasBorderForRightMovement;

            _previousHasBorderForLeftMovement = _hasBorderForLeftMovement;
            _previousHasBorderForRightMovement = _hasBorderForRightMovement;
        }
    }
}