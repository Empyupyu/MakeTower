using System;
using UnityEngine;
using Zenject;

namespace Source.Scripts.DragAndDrop
{
    public class DragService : IInitializable, IDisposable
    {
        private readonly RayCaster _rayCaster;
        private readonly DragObjectMover _dragObjectMover;

        public DragService(RayCaster rayCaster, DragObjectMover dragObjectMover)
        {
            _rayCaster = rayCaster;
            _dragObjectMover = dragObjectMover;
        }

        public void Initialize()
        {
            _rayCaster.OnClickDownRaycast += TryStartDrag;
        }
        
        private void TryStartDrag(Collider2D collider)
        {
            if(!collider.TryGetComponent<IDragAndDrop>(out var drag)) return;
            
            _dragObjectMover.Select(drag);
        }

        public void Dispose()
        {
            _rayCaster.OnClickDownRaycast -= TryStartDrag;
        }
    }
}