using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.Scripts.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [field: SerializeField] public Transform ItemContainer { get; private set; }
        [field: SerializeField] public Cube CubePrefab { get; private set; }
        [field: SerializeField] public float Spacing { get; private set; }
        [field: SerializeField] public float ScrollSpeed { get; private set; }
        
        private Vector3 _clickPosition;
        private Cube _cubeInRayCast;
        
        private CubeHolder _cubeHolder;
        private RayCaster _rayCaster;

        [Inject]
        private void Initialize(CubeHolder cubeHolder, RayCaster rayCaster)
        {
            _cubeHolder = cubeHolder;
            _rayCaster = rayCaster;
        }

        //TODO
        private void OnClickUp(Collider2D collider)
        {
            _cubeHolder.ClearHolder();
        }

        private void OnClickDown(Vector3 position)
        {
            _cubeInRayCast = null;
            _clickPosition = position;
        }
    }
}
