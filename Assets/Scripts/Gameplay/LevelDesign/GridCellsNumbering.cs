using Loderunner.Service;
using UnityEditor;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GridCellsNumbering : MonoBehaviour
    {
        [SerializeField] private float _zoomCompensation = 1;
        [SerializeField] private Vector2 _offset = new Vector2(1, 1);

        private void OnDrawGizmos()
        {
            Handles.BeginGUI();
            var textStyle = new GUIStyle();

            textStyle.normal.textColor = Color.green;
            var zoom = SceneView.currentDrawingSceneView.camera.orthographicSize;
            textStyle.fontSize = (int)(10 / zoom);

            var xOffset = _offset.x;
            var yOffset = _offset.y - zoom / _zoomCompensation;

            var cellSize = 0.16f;

            for (int i = 0; i < 23; i++)
            {
                for (int j = 0; j < 23; j++)
                {
                    var position = new Vector2(i * cellSize - xOffset + 0.02f, j * cellSize - yOffset + 0.08f);

                    Handles.Label(position, $"{i}:{j}", textStyle);
                }
            }

            Handles.EndGUI();
        }
    }
}