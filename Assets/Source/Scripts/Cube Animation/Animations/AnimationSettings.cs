using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSettings 1", menuName = "Settings/AnimationSettings")]
public class AnimationSettings : ScriptableObject
{
    [field: SerializeField] public float ShakeDuration { get; private set; }
    [field: SerializeField] public Vector3 Strength  { get; private set; }
    [field: SerializeField] public int Vibrato  { get; private set; }
    [field: SerializeField] public float Randomness  { get; private set; }
    [field: SerializeField] public Ease Ease  { get; private set; }
}