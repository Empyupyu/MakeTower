using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Inventory
{
    [CreateAssetMenu(fileName = "CubeInventory", menuName = "ScriptableObjects/CubeInventory", order = 1)]
    public class InventorySettings : ScriptableObject
    {
        /// <summary>
        /// 
        /// </summary>
        [field: SerializeField] public int InventoryCapacity { get; private set; }
        [field: SerializeField] public List<Sprite> CubeColors { get; private set; }
    }
}