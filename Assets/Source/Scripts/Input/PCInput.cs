using System;
using UnityEngine;

public class PCInput : IInput
{
    public event Action<Vector2> ClickDown;
    public event Action<Vector2> ClickUp;
    public event Action<Vector2> Drag;
    public event Action<Vector2> Delta;

    private Vector2 _lastPosition;

    public void Tick()
    {
        var click = (Vector2)Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            _lastPosition = click;
            ClickDown?.Invoke(click);
        }

        if (Input.GetMouseButton(0))
        {
            Drag?.Invoke(click);
            Delta?.Invoke(click - _lastPosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            ClickUp?.Invoke(click);
        }

        _lastPosition = click;
    }
}