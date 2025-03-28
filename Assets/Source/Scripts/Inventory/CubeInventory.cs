using System.Collections.Generic;

namespace Source.Scripts.Inventory
{
    public class CubeInventory
    {
        public List<Cube> Items { get; set; }

        public CubeInventory(List<Cube> items)
        {
            Items = items;
        }

        public void AddItem(Cube cube)
        {
            Items.Add(cube);
        }

        public void RemoveItem(Cube cube)
        {
            Items.Remove(cube);
        }
    }
}
