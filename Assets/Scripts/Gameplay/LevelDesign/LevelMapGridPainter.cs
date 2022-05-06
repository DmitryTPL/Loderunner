using System.Diagnostics;
using Loderunner.Service;
using UnityEditor;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelMapGridPainter : MonoBehaviour
    {
        [SerializeField] private float _zoomCompensation = 1;
        [SerializeField] private Vector2 _offset = new(1, 1);

        private Matrix<int> _map;

        private int _levelLength;
        private int _levelHeight;

        public void SetData(Matrix<int> map, int levelHeight, int levelLength)
        {
            _map = map;
            _levelHeight = levelHeight;
            _levelLength = levelLength;
        }

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos()
        {
            Handles.BeginGUI();

            if (_map == null || _map.Length == 0)
            {
                return;
            }

            var positiveNumberTextStyle = new GUIStyle
            {
                normal =
                {
                    textColor = Color.red
                }
            };
            
            var negativeNumberTextStyle = new GUIStyle
            {
                normal =
                {
                    textColor = Color.yellow
                }
            };

            var zoom = SceneView.currentDrawingSceneView.camera.orthographicSize;
            negativeNumberTextStyle.fontSize = (int)(10 / zoom);

            var xOffset = _offset.x;
            var yOffset = _offset.y - zoom / _zoomCompensation;

            for (var row = 0; row < _levelHeight; row++)
            {
                for (var column = 0; column < _levelLength; column++)
                {
                    if (_map[row, column] == 0)
                    {
                        continue;
                    }

                    var cellValue = _map[row, column];

                    var textStyle = cellValue >= 0 ? positiveNumberTextStyle : negativeNumberTextStyle;
                    
                    Handles.Label(new Vector2(column + xOffset, row - yOffset), $"{cellValue}", textStyle);
                }
            }

            Handles.EndGUI();
        }
    }
}