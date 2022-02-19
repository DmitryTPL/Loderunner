using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Loderunner.Service
{
    [RequireComponent(typeof(Camera))]
    public class BaseCameraController : MonoBehaviour
    {
        private UniversalAdditionalCameraData _cameraData;
        
        private void Awake()
        {
            _cameraData = gameObject.GetComponent<UniversalAdditionalCameraData>();
        }

        private void OnEnable()
        {
            foreach (var overlayCamera in CamerasHolder.GetOverlayCameras())
            {
                AddToStack(overlayCamera);
            }
        }
        
        private void OnDisable()
        {
            _cameraData.cameraStack.Clear();
        }

        public void AddToStack(Camera overlayCamera)
        {
            _cameraData.cameraStack.Add(overlayCamera);
        }

        public void RemoveFromStack(Camera overlayCamera)
        {
            _cameraData.cameraStack.Remove(overlayCamera);
        }
    }
}
