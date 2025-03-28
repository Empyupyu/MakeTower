using UnityEngine;

[CreateAssetMenu(fileName = "RemoveFromTowerAnimationSettings 1", menuName = "Settings/RemoveFromTowerAnimationSettings")]
public class RemoveFromTowerAnimationSettings : AnimationSettings
{
    [field: SerializeField] public float Duration { get; private set; }
   
}