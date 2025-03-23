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
        private readonly CubeHolder _cubeHolder;
        private List<Cube> _cubes;
        private float _delayForGetCube = .1f;
        private readonly RayCaster _rayCaster;
        private Cube _cubeInRayCast;

        public InventoryController(CubeInventory inventory, InventoryView inventoryView,
            InventorySettings inventorySettings, IInput input, CubeHolder cubeHolder, RayCaster rayCaster)
        {
            _inventory = inventory;
            _inventoryView = inventoryView;
            _inventorySettings = inventorySettings;
            _input = input;
            _cubeHolder = cubeHolder;
            _cubes = new List<Cube>();
            _rayCaster = rayCaster;

            _input.Delta += TryGetCube;
            _input.ClickDown += CheckCube;
            // _input.Delta().Subscribe(TryGetCube);
            // Observable.
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

                // var cube = GameObject.Instantiate(_inventoryView.CubePrefab);
                // cube.Visual.sprite = item.CubeColor;
                // _cubeHolder.SetSelectedCube(cube);
            }
        }

        private void TryGetCube(Vector2 delta)
        {
            if (_cubeHolder.SelectedCube != null) return;

            if (delta != Vector2.zero)
            {
                var direction = delta.normalized;

                _delayForGetCube -= Time.deltaTime;

                if (_delayForGetCube > 0)
                {
                    if (direction.y >= .5f)
                    {
                        if (_cubeInRayCast != null)
                        {
                            _cubeHolder.SetSelectedCube(GameObject.Instantiate(_cubeInRayCast));

                            return;
                        }
                    }

                    return;
                }

                _cubeInRayCast = null;

                direction *= _inventoryView.ScrollSpeed * Time.deltaTime;
                direction.y = 0;

                direction.x = Mathf.Clamp(_inventoryView.ItemContainer.transform.localPosition.x + direction.x, -_cubes.Count * _inventoryView.Spacing,
                    -11f);
                _inventoryView.ItemContainer.transform.localPosition = direction;
            }
        }
        
        private void CheckCube(Vector2 _clickPosition)
        {
            var ray = Camera.main.ScreenPointToRay(_clickPosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                hit.collider.TryGetComponent(out _cubeInRayCast);

                if (_cubeInRayCast != null)
                {
                    _delayForGetCube = .1f;
                }
            }
        }
    }
}
