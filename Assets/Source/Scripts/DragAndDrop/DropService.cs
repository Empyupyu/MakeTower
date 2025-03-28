using System;
using UnityEngine;
using Zenject;

namespace Source.Scripts.DragAndDrop
{
    public class DropService : IInitializable, IDisposable
    {
        private readonly DragObjectMover _dragObjectMover;
        private readonly RayCaster _caster;

        public DropService(DragObjectMover dragObjectMover, RayCaster caster)
        {
            _dragObjectMover = dragObjectMover;
            _caster = caster;
        }
        
        public void Initialize()
        {
            _caster.OnClickUpRaycast += TryDrop;
        }
        
        private void TryDrop(Collider2D collider)
        {
            if(collider == null) return;
            
            collider.TryGetComponent<IDropZone>(out var dropZone);

            if (dropZone == null) return;

            if (_dragObjectMover.SelectedDragAndDrop == null) return;

            dropZone.Drop(_dragObjectMover.SelectedDragAndDrop);
            _dragObjectMover.Deselect();
        }

        public void Dispose()
        {
            _caster.OnClickUpRaycast -= TryDrop;
        }
    }
}