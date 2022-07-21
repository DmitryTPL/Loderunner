using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollowView : View<CameraFollowPresenter>
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Start()
        {
            _presenter.SetOrthographicSize(_camera.orthographicSize, _camera.aspect);
        }

        private void FixedUpdate()
        {
            transform.position = _presenter?.GetNewCameraPosition(transform.position) ?? Vector3.zero;
        }
    }
}