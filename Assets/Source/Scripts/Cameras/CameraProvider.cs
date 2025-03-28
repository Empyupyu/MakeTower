using UnityEngine;

namespace Source.Scripts.Cameras
{
    public class CameraProvider : ICameraProvider
    {
        public Camera MainCamera { get; private set; }

        public CameraProvider(Camera camera)
        {
            MainCamera = camera;
        }
    }
}