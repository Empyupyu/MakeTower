using DG.Tweening;
using UnityEngine;

namespace Source.Scripts.CameraShaker
{
    public class CameraShaker
    {
        public void Shake(float duration, Vector3 strength, int vibrato, float randomness, Ease ease)
        {
            Camera.main.transform.DOShakePosition(duration, strength, vibrato, randomness)
                .SetEase(ease);
        }
    }
}