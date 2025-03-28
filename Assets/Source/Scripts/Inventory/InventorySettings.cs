using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Inventory
{
    [CreateAssetMenu(fileName = "InventorySettings", menuName = "ScriptableObjects/InventorySettings", order = 1)]
    public class InventorySettings : ScriptableObject
    {
        [field: SerializeField] public int InventoryCapacity { get; private set; }
        [field: SerializeField] public List<Sprite> CubeColors { get; private set; }
    }
}