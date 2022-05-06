using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GridCellsNumbering : MonoBehaviour
    {
        [SerializeField] private float _zoomCompensation = 1;
        [SerializeField] private Vector2 _offset = new (1, 1);
        [SerializeField, Range(1, 100)] private int _levelLength;
        [SerializeField, Range(1, 100)] private int _levelHeight;

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos()
        {
            Handles.BeginGUI();
            
            var textStyle = new GUIStyle
            {
                normal =
                {
                    textColor = Color.green
                },
                fontSize = 6
            };

            var zoom = SceneView.currentDrawingSceneView.camera.orthographicSize;
            textStyle.fontSize = (int)(10 / zoom);

            var xOffset = _offset.x;
            var yOffset = _offset.y - zoom / _zoomCompensation;

            for (var row = 0; row < _levelHeight; row++)
            {
                for (var column = 0; column < _levelLength; column++)
                {
                    Handles.Label(new Vector2(column + xOffset, row - yOffset), $"{column}:{row}", textStyle);
                }
            }

            Handles.EndGUI();
        }
    }
}