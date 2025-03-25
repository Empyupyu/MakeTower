using System;
using Source.Scripts.Interface;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Drop
{
    public class DropService : IDisposable
    {
        private readonly CubeMover _cubeMover;
        private readonly RayCaster _caster;

        public DropService(CubeMover cubeMover, RayCaster caster)
        {
            _cubeMover = cubeMover;
            _caster = caster;
            
            _caster.OnClickUpRaycast += OnClickUpRaycast;
        }
        
        private void OnClickUpRaycast(Collider2D collider)
        {
            if (collider == null) return;

            collider.TryGetComponent<IDropZone>(out var dropZone);

            if (dropZone == null) return;

            if (_cubeMover.SelectedCube == null) return;

            dropZone.Drop(_cubeMover.SelectedCube);
        }

        public void Dispose()
        {
            _caster.OnClickUpRaycast -= OnClickUpRaycast;
        }
    }
}