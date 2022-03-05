using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    public class WallPlacer : PlacerBase
    {
        [Header("Blocks")]
        [SerializeField, Range(0, 23)] private int _blocksCountHorizontal;
        [SerializeField, Range(0, 23)] private int _blocksCountVertical;
        [SerializeField] private WallBlockView _prefab;
        [SerializeField] private List<HorizontalBlocks> _blocks = new List<HorizontalBlocks>();
        
        [Space, Header("Fall")]
        [SerializeField] private bool _canFallFromLeft;
        [SerializeField] private bool _canFallFromRight;
        [SerializeField] private BoxCollider2D _leftFallingPointCollider;
        [SerializeField] private BoxCollider2D _rightFallingPointCollider;
        [SerializeField] private BoxCollider2D _mainCollider;
        
        [Space, Header("Border")]
        [SerializeField] private bool _hasBorderForLeftMovement;
        [SerializeField] private bool _hasBorderForRightMovement;
        [SerializeField] private BoxCollider2D _borderForLeftMovementCollider;
        [SerializeField] private BoxCollider2D _borderForRightMovementCollider;

        private int _previousBlocksCountHorizontal;
        private int _previousBlocksCountVertical;
        private bool _previousCanFallLeft;
        private bool _previousCanFallRight;
        private bool _previousHasBorderForLeftMovement;
        private bool _previousHasBorderForRightMovement;

        protected override void OnEnable()
        {
            base.OnEnable();

            _previousBlocksCountHorizontal = _blocksCountHorizontal;
            _previousBlocksCountVertical = _blocksCountVertical;
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
            var blocksVertical = _blocksCountVertical;
            var blocksHorizontal = _blocksCountHorizontal;

            _blocksCountVertical = 0;
            _blocksCountHorizontal = 0;

            Update();

            _blocksCountVertical = blocksVertical;
            _blocksCountHorizontal = blocksHorizontal;

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
            if (_previousBlocksCountHorizontal != _blocksCountHorizontal || _previousBlocksCountVertical != _blocksCountVertical)
            {
                if (_blocksCountVertical == 0 && _blocksCountHorizontal > 0)
                {
                    _blocksCountVertical = 1;
                }

                if (_blocks.Count > _blocksCountVertical)
                {
                    for (var k = _blocksCountVertical; k < _blocks.Count; k++)
                    {
                        foreach (var block in _blocks[k].Blocks)
                        {
                            DestroyImmediate(block.gameObject);
                        }
                    }

                    _blocks.RemoveRange(_blocksCountVertical, _blocks.Count - _blocksCountVertical);
                }
                else
                {
                    for (var i = 0; i < _blocksCountVertical; i++)
                    {
                        if (_blocks.Count <= i)
                        {
                            _blocks.Add(new HorizontalBlocks());
                        }

                        var horizontal = _blocks[i];

                        if (horizontal.Blocks.Count > _blocksCountHorizontal)
                        {
                            for (var j = _blocksCountHorizontal; j < horizontal.Blocks.Count; j++)
                            {
                                DestroyImmediate(horizontal.Blocks[j].gameObject);
                            }

                            horizontal.Blocks.RemoveRange(_blocksCountHorizontal, horizontal.Blocks.Count - _blocksCountHorizontal);
                        }
                        else
                        {
                            var blocksCount = horizontal.Blocks.Count;

                            for (var j = blocksCount; j < _blocksCountHorizontal; j++)
                            {
                                var block = PrefabUtility.InstantiatePrefab(_prefab) as WallBlockView;

                                block.transform.position = new Vector3(transform.position.x + j * CellSize, transform.position.y + i * CellSize);
                                block.transform.parent = transform;

                                horizontal.Blocks.Add(block);
                            }
                        }
                    }
                }

                _previousBlocksCountHorizontal = _blocksCountHorizontal;
                _previousBlocksCountVertical = _blocksCountVertical;

                RecalculateFallEdges();
            }
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

            var mainColliderSize = CellSize * _blocks[^1].Blocks.Count;
            var mainColliderOffset = mainColliderSize / 2;

            if (_canFallFromLeft)
            {
                mainColliderSize -= _leftFallingPointCollider.size.x;
                mainColliderOffset += _leftFallingPointCollider.size.x / 2;
                _leftFallingPointCollider.offset =
                    new Vector2(_leftFallingPointCollider.offset.x, CellSize * _blocks.Count - _leftFallingPointCollider.size.y / 2);
                _leftFallingPointCollider.enabled = _canFallFromLeft;
            }

            if (_canFallFromRight)
            {
                mainColliderSize -= _rightFallingPointCollider.size.x;
                mainColliderOffset -= _rightFallingPointCollider.size.x / 2;
                _rightFallingPointCollider.offset =
                    new Vector2(CellSize * _blocks[^1].Blocks.Count - _rightFallingPointCollider.size.x / 2,
                        CellSize * _blocks.Count - _rightFallingPointCollider.size.y / 2);
                _rightFallingPointCollider.enabled = _canFallFromRight;
            }

            _mainCollider.size = new Vector2(mainColliderSize, _mainCollider.size.y);
            _mainCollider.offset = new Vector2(mainColliderOffset, CellSize * _blocks.Count - _mainCollider.size.y / 2);
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
            if (_blocks.Count == 0 || _blocks[0].Blocks.Count == 0)
            {
                return;
            }
            
            var offset = CellSize / 8;
            
            if (_hasBorderForLeftMovement)
            {
                _borderForLeftMovementCollider.enabled = _hasBorderForLeftMovement;
                
                _borderForLeftMovementCollider.size = new Vector2(CellSize / 2, CellSize * _blocks.Count - offset);
                _borderForLeftMovementCollider.offset = new Vector2(CellSize * _blocks[0].Blocks.Count - _borderForLeftMovementCollider.size.x / 2,
                    _borderForLeftMovementCollider.size.y / 2 + offset / 2);
            }
            
            if (_hasBorderForRightMovement)
            {
                _borderForRightMovementCollider.enabled = _hasBorderForRightMovement;
                
                _borderForRightMovementCollider.size = new Vector2(CellSize / 2, CellSize * _blocks.Count - offset);
                _borderForRightMovementCollider.offset = new Vector2(_borderForRightMovementCollider.size.x / 2,
                    _borderForRightMovementCollider.size.y / 2 + offset / 2);
            }

            _previousHasBorderForLeftMovement = _hasBorderForLeftMovement;
            _previousHasBorderForRightMovement = _hasBorderForRightMovement;
        }
    }
}