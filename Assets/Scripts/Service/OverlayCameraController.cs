using UnityEngine;

namespace Loderunner.Service
{
    [RequireComponent(typeof(Camera))]
    public class OverlayCameraController : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = gameObject.GetComponent<Camera>();
        }

        private void OnEnable()
        {
            CamerasHolder.AddCamera(_camera);
        }

        private void OnDisable()
        {
            CamerasHolder.RemoveCamera(_camera);
        }
    }
}