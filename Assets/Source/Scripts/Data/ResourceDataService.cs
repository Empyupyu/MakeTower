using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Inventory
{
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