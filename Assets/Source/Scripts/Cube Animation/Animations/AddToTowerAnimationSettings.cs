using UnityEngine;

[CreateAssetMenu(fileName = "AddToTowerAnimationSettings 1", menuName = "Settings/AddToTowerAnimationSettings")]
public class AddToTowerAnimationSettings : AnimationSettings
{
    [field: SerializeField] public float JumpPower { get; private set; }
    [field: SerializeField] public int NumJumps { get; private set; }
    [field: SerializeField] public float Duration { get; private set; }
   
}