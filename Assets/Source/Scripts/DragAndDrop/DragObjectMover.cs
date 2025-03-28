using System;
using Source.Scripts.Cameras;
using Source.Scripts.DragAndDrop;
using UnityEngine;
using Zenject;

public class DragObjectMover : IInitializable, IDisposable
{
    public event Action<IDragAndDrop> OnSelect;
    public event Action<IDragAndDrop> OnDeselect;
    public Vector3 DragOffset { get; private set; }
    public IDragAndDrop SelectedDragAndDrop { get; private set; }

    private readonly IInput _input;
    private readonly ICameraProvider _cameraProvider;
    private Transform _transformDragObject;

    public DragObjectMover(IInput input, ICameraProvider cameraProvider)
    {
        _input = input;
        _cameraProvider = cameraProvider;
    }
    
    public void Initialize()
    {
        _input.Drag += UpdatePosition;
    }

    public void Select(IDragAndDrop dragObject)
    {
        var obj = dragObject.GetGameObject();

        SelectedDragAndDrop = dragObject;
        _transformDragObject = SelectedDragAndDrop.GetGameObject().transform;
        EnableCollider(false);
        
        OnSelect?.Invoke(SelectedDragAndDrop);
    }

    public void Deselect()
    {
        EnableCollider(true);

        OnDeselect?.Invoke(SelectedDragAndDrop);
        SelectedDragAndDrop = null;
        _transformDragObject = null;
    }

    private void EnableCollider(bool enabled)
    {
        _transformDragObject.GetComponent<Collider2D>().enabled = enabled;
    }
    
    private void UpdatePosition(Vector2 inputPosition)
    {
        if (SelectedDragAndDrop == null) return;

        var worldPoint = _cameraProvider.MainCamera.ScreenToWorldPoint(inputPosition);
        worldPoint.z = _transformDragObject.position.z;
        _transformDragObject.position = worldPoint + DragOffset;
    }

    public void Dispose()
    {
        _input.Drag -= UpdatePosition;
    }
}