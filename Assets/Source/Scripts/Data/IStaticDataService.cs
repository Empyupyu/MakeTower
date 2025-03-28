using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Inventory
{
    public interface IStaticDataService
    {
        public void Load();
        public int GetInventoryCapacity();
        public List<Sprite> GetInventoryColors();
    }
}