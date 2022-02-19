using System.Collections.Generic;
using Loderunner.Service;
using UnityEngine;
using Vector2Int = UnityEngine.Vector2Int;

namespace Loderunner.Gameplay.LevelDesign
{
    [ExecuteInEditMode]
    public class LevelUnitPlacer : MonoBehaviour
    {
        [SerializeField, Range(0, 23)] private int _blocksCountHorizontal;
        [SerializeField, Range(0, 23)] private int _blocksCountVertical;
        [SerializeField] private Vector2Int _startingCellPosition;
        [SerializeField] private GameObject _prefab;
        [SerializeField, HideInInspector] private List<HorizontalBlocks> _blocks = new List<HorizontalBlocks>();

        private int _previousBlocksCountHorizontal;
        private int _previousBlocksCountVertical;
        private Vector2 _previousCellPosition;

        private void OnEnable()
        {
            _previousBlocksCountHorizontal = _blocksCountHorizontal;
            _previousBlocksCountVertical = _blocksCountVertical;
            _previousCellPosition = _startingCellPosition;
        }

        private void Update()
        {
            if (_previousCellPosition != _startingCellPosition)
            {
                _previousCellPosition = _startingCellPosition;
                transform.localPosition = new Vector3(_startingCellPosition.x * GlobalConstant.CellValue, _startingCellPosition.y * GlobalConstant.CellValue, 0);
            }

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
                            for (var k = _blocksCountHorizontal; k < horizontal.Blocks.Count; k++)
                            {
                                DestroyImmediate(horizontal.Blocks[k].gameObject);
                            }

                            horizontal.Blocks.RemoveRange(_blocksCountHorizontal, horizontal.Blocks.Count - _blocksCountHorizontal);
                        }
                        else
                        {
                            var blocksCount = horizontal.Blocks.Count;

                            for (var j = blocksCount; j < _blocksCountHorizontal; j++)
                            {
                                var block = Instantiate(_prefab,
                                    new Vector3(transform.position.x + j * GlobalConstant.CellValue, transform.position.y + i * GlobalConstant.CellValue),
                                    Quaternion.identity, transform);

                                horizontal.Blocks.Add(block);
                            }
                        }
                    }
                }

                _previousBlocksCountHorizontal = _blocksCountHorizontal;
                _previousBlocksCountVertical = _blocksCountVertical;
            }
        }
    }
}