using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Loderunner.Service
{
    public static class CamerasHolder
    {
        private static IList<Camera> _cameras = new List<Camera>();

        public static void AddCamera(Camera camera)
        {
            if (camera.GetUniversalAdditionalCameraData().renderType != CameraRenderType.Overlay)
            {
                Console.LogError("Camera must be of Overlay render type");
                return;
            }
            
            _cameras.Add(camera);
            
            if (Camera.main != null)
            {
                Camera.main.gameObject.GetComponent<BaseCameraController>().AddToStack(camera);
            }
        }
        
        public static void RemoveCamera(Camera camera)
        {
            if (camera.GetUniversalAdditionalCameraData().renderType != CameraRenderType.Overlay)
            {
                Console.LogError("Camera must be of Overlay render type");
                return;
            }
            
            _cameras.Remove(camera);
            
            if (Camera.main != null)
            {
                Camera.main.gameObject.GetComponent<BaseCameraController>().RemoveFromStack(camera);
            }
        }

        public static IEnumerable<Camera> GetOverlayCameras()
        {
            return _cameras;
        }
    }
}