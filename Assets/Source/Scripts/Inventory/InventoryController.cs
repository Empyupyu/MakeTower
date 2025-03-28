using System;
using Source.Scripts.ObjectPools;
using Source.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Inventory
{
    public class InventoryController : IInitializable, IDisposable
    {
        private readonly InventoryView _inventoryView;
        private readonly InventorySettings _inventorySettings;
        private readonly IInput _input;
        private readonly DragObjectMover _dragObjectMover;
        private readonly CubeObjectPoolProvider _objectPoolProvider;

        public InventoryController(InventoryView inventoryView,
            InventorySettings inventorySettings, IInput input, DragObjectMover dragObjectMover,
            CubeObjectPoolProvider objectPoolProvider)
        {
            _inventoryView = inventoryView;
            _inventorySettings = inventorySettings;
            _input = input;
            _dragObjectMover = dragObjectMover;
            _objectPoolProvider = objectPoolProvider;
        }

        public void Initialize()
        {
            _input.ClickUp += ActivateScroll;

            CreateInventory();
        }

        private void ActivateScroll(Vector2 position)
        {
            EnableScrollRect(true);
        }

        private void CreateInventory()
        {
            for (int i = 0; i < _inventorySettings.InventoryCapacity; i++)
            {
                var item = GameObject.Instantiate(_inventoryView.CubeViewPrefab,
                    _inventoryView.ItemContainer.transform);

                var colorIndex = GetColorIndex(i);
                item.CubeButton.image.sprite = _inventorySettings.CubeColors[colorIndex];

                item.OnCubeDrag += () => TryGetCube(colorIndex, item.transform.position);
            }

            _inventoryView.ScrollRect.horizontalNormalizedPosition = 0f;
            _inventoryView.ScrollRect.verticalNormalizedPosition = 1f;
        }

        private int GetColorIndex(int i)
        {
            var colorIndex = i < _inventorySettings.InventoryCapacity - 1
                ? i
                : i % _inventorySettings.InventoryCapacity;

            return colorIndex;
        }

        private void EnableScrollRect(bool enable)
        {
            _inventoryView.ScrollRect.enabled = enable;
        }

        private void TryGetCube(int colorIndex, Vector3 position)
        {
            if (_dragObjectMover.SelectedDragAndDrop != null) return;

            EnableScrollRect(false);
            
            var newCube = _objectPoolProvider.ObjectPool.Get();
            
            newCube.transform.localScale = Vector3.one;
            var spawnPosition = position;
            spawnPosition.z = 0;
            newCube.transform.position = spawnPosition;

            newCube.ColorID = colorIndex;
            newCube.Visual.sprite = _inventorySettings.CubeColors[colorIndex];

            _dragObjectMover.Select(newCube);
        }

        public void Dispose()
        {
            _input.ClickUp -= ActivateScroll;
        }
    }
}