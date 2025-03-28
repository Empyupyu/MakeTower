using Source.Scripts.DragAndDrop;
using UnityEngine;

namespace Source.Scripts.Inventory
{
    public class Cube : MonoBehaviour, IDragAndDrop
    {
        [field: SerializeField] public SpriteRenderer Visual { get; private set; }
        [field: SerializeField] public int ColorID { get;  set; }
        
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}