using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Inventory
{
    [CreateAssetMenu(fileName = "CubeInventory", menuName = "ScriptableObjects/CubeInventory", order = 1)]
    public class InventorySettings : ScriptableObject
    {
        [field: SerializeField] public int InventoryCapacity { get; private set; }
        [field: SerializeField] public List<Sprite> CubeColors { get; private set; }
    }

    public interface IStaticDataService
    {
       public void Load();
       public int GetInventoryCapacity();
       public List<Sprite> GetInventoryColors();
    }

    public class ResourceDataService : IStaticDataService
    {
        private InventorySettings _inventorySettings;

        public void Load()
        {
            LoadInventorySettings();
        }

        public int GetInventoryCapacity()
        {
            return _inventorySettings.InventoryCapacity;
        }
        
        public List<Sprite> GetInventoryColors()
        {
            return _inventorySettings.CubeColors;
        }

        private void LoadInventorySettings()
        {
            _inventorySettings = Resources.Load<InventorySettings>("InventorySettings");
        }
    }
}