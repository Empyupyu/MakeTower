using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionAnimationSettings 1", menuName = "Settings/ExplosionAnimationSettings")]
public class ExplosionAnimationSettings : AnimationSettings
{
    [field: SerializeField] public float ScalingDuration { get; private set; }
   
}