using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    public class InventoryController
    {
        private readonly InventoryView _inventoryView;
        private readonly CubeInventory _inventory;
        private readonly InventorySettings _inventorySettings;
        private readonly IInput _input;
        private readonly CubeMover _cubeMover;
        private List<Cube> _cubes;
        private float _delayForGetCube = .1f;
        private readonly RayCaster _rayCaster;
        private Cube _cubeInRayCast;

        public InventoryController(CubeInventory inventory, InventoryView inventoryView,
            InventorySettings inventorySettings, IInput input, CubeMover cubeMover, RayCaster rayCaster)
        {
            _inventory = inventory;
            _inventoryView = inventoryView;
            _inventorySettings = inventorySettings;
            _input = input;
            _cubeMover = cubeMover;
            _cubes = new List<Cube>();
            _rayCaster = rayCaster;

            _input.Delta += TryGetCube;
            _rayCaster.OnClickDownRaycast += CheckCube;
        }

        public void CreateInventory()
        {
            for (int i = 0; i < _inventorySettings.InventoryCapacity; i++)
            {
                var item = GameObject.Instantiate(_inventoryView.CubePrefab, _inventoryView.ItemContainer.transform);
                item.transform.localPosition = new Vector3(i * _inventoryView.Spacing, item.transform.localPosition.y,
                    item.transform.localPosition.z);

                var colorIndex = i < _inventorySettings.InventoryCapacity - 1
                    ? i
                    : i % _inventorySettings.InventoryCapacity;

                item.Visual.sprite = (_inventorySettings.CubeColors[colorIndex]);
                _cubes.Add(item);
            }
        }
        
        private void CheckCube(Collider2D collider)
        {
            if (collider == null) return;
            
            collider.TryGetComponent(out _cubeInRayCast);

            if (_cubeInRayCast == null) return;
            
            _delayForGetCube = .1f;
        }

        //TODO
        private void TryGetCube(Vector2 delta)
        {
            if (_cubeMover.SelectedCube != null) return;

            if (delta != Vector2.zero)
            {
                var direction = delta.normalized;
                _delayForGetCube -= Time.deltaTime; //TODO

                if (_delayForGetCube > 0)
                {
                    if (direction.y >= .5f)
                    {
                        if (_cubeInRayCast != null && _cubeInRayCast.InWorld == false)
                        {
                            var cube = GameObject.Instantiate(_cubeInRayCast);
                            cube.InWorld = true;
                            _cubeMover.SetSelectedCube(cube);

                            return;
                        }
                    }

                    return;
                }

                _cubeInRayCast = null;

                Scrolling(direction);
            }
        }

        private void Scrolling(Vector2 direction)
        {
            direction *= _inventoryView.ScrollSpeed * Time.deltaTime;
            direction.y = 0;

            //TODO
            direction.x = Mathf.Clamp(_inventoryView.ItemContainer.transform.localPosition.x + direction.x, -_cubes.Count * _inventoryView.Spacing,
                -11f);
            _inventoryView.ItemContainer.transform.localPosition = direction;
        }
    }
}
