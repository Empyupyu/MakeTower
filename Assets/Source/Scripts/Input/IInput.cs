using System;
using UnityEngine;
using Zenject;

public interface IInput : ITickable
{
    public event Action<Vector2> ClickDown;
    public event Action<Vector2> ClickUp;
    public event Action<Vector2> Drag;
    public event Action<Vector2> Delta;
}