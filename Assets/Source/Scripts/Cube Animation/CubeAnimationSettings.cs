using Source.Scripts.Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "CubeAnimationSettings", menuName = "Settings/CubeAnimationSettings")]
public class CubeAnimationSettings : ScriptableObject
{
    [field: SerializeField] public Cube CubePrefab { get; private set; }
    [field: SerializeField] public ExplosionAnimationSettings ExplosionAnimationSettings { get; private set; }
    [field: SerializeField] public AddToTowerAnimationSettings AddToTowerAnimationSettings { get; private set; }
    [field: SerializeField] public RemoveFromTowerAnimationSettings RemoveFromTowerAnimationSettings { get; private set; }
    [field: SerializeField] public DropToHoleAnimationSettings DropToHoleAnimationSettings { get; private set; }
}