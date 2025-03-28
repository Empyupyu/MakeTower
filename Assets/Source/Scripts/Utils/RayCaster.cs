using System;
using Source.Scripts.Cameras;
using Source.Scripts.Extensions;
using UnityEngine;

public class RayCaster : IDisposable
{
    public Action<Collider2D> OnClickDownRaycast;
    public Action<Collider2D> OnClickUpRaycast;
    
    private readonly IInput _input;
    private readonly ICameraProvider _cameraProvider;
    
    private RaycastHit2D _hit;

    public RayCaster(IInput input, ICameraProvider cameraProvider)
    {
        _input = input;
        _cameraProvider = cameraProvider;

        _input.ClickDown += OnClickDown;
        _input.ClickUp += OnClickUp;
    }

    public void Dispose()
    {
        _input.ClickDown -= OnClickDown;
        _input.ClickUp -= OnClickUp;
    }

    private void OnClickDown(Vector2 position)
    {
        _hit = RaycastHit2D(position, CollisionLayers.Draggable.AsMask());

        if (_hit.collider != null)
            OnClickDownRaycast?.Invoke(_hit.collider);
    }

    private void OnClickUp(Vector2 position)
    {
        _hit = RaycastHit2D(position, CollisionLayers.DropZone.AsMask());

        OnClickUpRaycast?.Invoke(_hit.collider);
    }

    private RaycastHit2D RaycastHit2D(Vector3 position, int layerMask = default)
    {
        var ray = _cameraProvider.MainCamera.ScreenPointToRay(position);
        var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);
        return hit;
    }
}