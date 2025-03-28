using DG.Tweening;
using Source.Scripts.Cameras;
using UnityEngine;

namespace Source.Scripts.Utils
{
    public class CameraShaker
    {
        private readonly ICameraProvider _cameraProvider;
        private Camera _mainCamera;

        public CameraShaker(ICameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }
        
        public void Shake(float duration, Vector3 strength, int vibrato, float randomness, Ease ease)
        {
            _cameraProvider.MainCamera.transform.DOShakePosition(duration, strength, vibrato, randomness)
                .SetEase(ease);
        }
    }
}