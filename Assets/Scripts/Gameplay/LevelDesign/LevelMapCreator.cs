using System.Collections.Generic;
using System.Linq;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    public class LevelMapCreator : MonoBehaviour
    {
        public const int CanMove = 1;
        public const int Empty = 0;
        public const int Wall = -1;
        public const int Crossbar = 2;
        
        [SerializeField, Range(1, 100)] private int _levelLength;
        [SerializeField, Range(1, 100)] private int _levelHeight;

        [SerializeField] private LevelMapGridPainter _gridPainter;

        public Matrix<int> CreateMap(IEnumerable<PlacerBase> placers)
        {
            var map = new Matrix<int>(_levelHeight, _levelLength);

            foreach (var placer in placers)
            {
                switch (placer)
                {
                    case WallPlacer wallPlacer:
                        SetupFloorValues(map, wallPlacer);
                        break;
                    case LadderPlacer ladderPlacer:
                        SetupLadderValues(map, ladderPlacer);
                        break;
                }
            }

            foreach (var wallPlacer in placers.Where(p => p is WallPlacer))
            {
                SetupWallValues(map, wallPlacer as WallPlacer);
            }
            
            foreach (var crossbarPlacer in placers.Where(p => p is CrossbarPlacer))
            {
                SetupCrossbarValues(map, crossbarPlacer as CrossbarPlacer);
            }

            _gridPainter.SetData(map, _levelHeight, _levelLength);

            return map;
        }

        private void SetupFloorValues(Matrix<int> map, WallPlacer wallPlacer)
        {
            for (var cellIndex = 0; cellIndex < wallPlacer.BlocksCount; cellIndex++)
            {
                map[wallPlacer.CellPosition.y + 1, wallPlacer.CellPosition.x + cellIndex] = CanMove;
            }
        }

        private void SetupWallValues(Matrix<int> map, WallPlacer wallPlacer)
        {
            for (var cellIndex = 0; cellIndex < wallPlacer.BlocksCount; cellIndex++)
            {
                map[wallPlacer.CellPosition.y, wallPlacer.CellPosition.x + cellIndex] = Wall;
            }
        }

        private void SetupLadderValues(Matrix<int> map, LadderPlacer ladderPlacer)
        {
            for (var cellIndex = 0; cellIndex <= ladderPlacer.Height; cellIndex++)
            {
                if (ladderPlacer.CellPosition.y + cellIndex >= _levelHeight)
                {
                    continue;
                }

                map[ladderPlacer.CellPosition.y + cellIndex, ladderPlacer.CellPosition.x] = CanMove;
            }
        }

        private void SetupCrossbarValues(Matrix<int> map, CrossbarPlacer crossbarPlacer)
        {
            for (var cellIndex = 0; cellIndex <= crossbarPlacer.Length; cellIndex++)
            {
                if (crossbarPlacer.CellPosition.x + cellIndex >= _levelLength)
                {
                    continue;
                }

                var cellPositionX = crossbarPlacer.CellPosition.x + cellIndex;

                map[crossbarPlacer.CellPosition.y, cellPositionX] = CanMove;

                for (var i = 1; i < _levelHeight; i++)
                {
                    var nextPositionY = crossbarPlacer.CellPosition.y - i;

                    if (nextPositionY <= 0)
                    {
                        break;
                    }
                    
                    var cellValue = map[nextPositionY, cellPositionX];

                    if (cellValue != 0)
                    {
                        break;
                    }

                    map[nextPositionY, cellPositionX] = Crossbar;
                }
            }
        }
    }
}