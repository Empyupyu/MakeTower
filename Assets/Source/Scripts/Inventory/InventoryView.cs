using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [field: SerializeField] public Transform ItemContainer { get; private set; }
        [field: SerializeField] public CubeView CubeViewPrefab { get; private set; }
        [field: SerializeField] public ScrollRect ScrollRect { get; private set; }
    }
}
