using System;
using UnityEngine;

public class MobileInput : IInput
{
    public event Action<Vector2> ClickDown;
    public event Action<Vector2> ClickUp;
    public event Action<Vector2> Drag;
    public event Action<Vector2> Delta;

    private Vector2 _lastPosition;

    public void Tick()
    {
        if (Input.touchCount == 0) return;

        var touch = Input.touches[0];
        _lastPosition = touch.position;

        Delta?.Invoke(touch.position - _lastPosition);

        if (touch.phase == TouchPhase.Began)
        {
            Drag?.Invoke(touch.position);
        }

        if (touch.phase == TouchPhase.Moved)
        {
            Drag?.Invoke(touch.position);
        }

        if (touch.phase == TouchPhase.Ended)
        {
            ClickUp?.Invoke(touch.position);
        }
    }
}