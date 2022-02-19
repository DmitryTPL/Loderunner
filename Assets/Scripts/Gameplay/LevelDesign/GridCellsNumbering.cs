using UnityEditor;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GridCellsNumbering : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Handles.BeginGUI();
            var textStyle = new GUIStyle();
            
            textStyle.normal.textColor = Color.green;
            var zoom = SceneView.currentDrawingSceneView.camera.orthographicSize;
            textStyle.fontSize = (int)(10 / zoom);

            var xOffset = 2.72f;
            var yOffset = 2.01f - zoom / 4.5f;
            
            for (int i = 0; i < 23; i++)
            {
                for (int j = 0; j < 23; j++)
                {
                    var position = new Vector2(i * 0.16f - xOffset + 0.02f, j * 0.16f - yOffset + 0.08f);
                    
                    Handles.Label(position, $"{i}:{j}", textStyle);
                }
            }
            
            Handles.EndGUI();
        }
    }
}