using UnityEngine;

[CreateAssetMenu(fileName = "DropToHoleAnimationSettings 1", menuName = "Settings/DropToHoleAnimationSettings")]
public class DropToHoleAnimationSettings : AnimationSettings
{
    [field: SerializeField] public float Duration { get; private set; }
    [field: SerializeField] public float OffsetY { get; private set; }
   
}