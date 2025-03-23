using UnityEngine;

namespace Source.Scripts.Inventory
{
    public class Cube : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer Visual { get; private set; }
    }
}