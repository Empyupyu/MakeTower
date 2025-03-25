using Source.Scripts.Interface;
using UnityEngine;

namespace Source.Scripts.Inventory
{
    public class Cube : MonoBehaviour, IDrop
    {
        [field: SerializeField] public SpriteRenderer Visual { get; private set; }
        [field: SerializeField] public bool InWorld { get;  set; }
        
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}