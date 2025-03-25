using UnityEngine;

namespace Source.Scripts.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [field: SerializeField] public Transform ItemContainer { get; private set; }
        [field: SerializeField] public Cube CubePrefab { get; private set; }
        [field: SerializeField] public float Spacing { get; private set; }
        [field: SerializeField] public float ScrollSpeed { get; private set; }
    }
}
