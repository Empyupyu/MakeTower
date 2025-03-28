using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CubeView : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnCubeDrag;
    [field: SerializeField] public Button CubeButton { get; private set; }

    private bool _select;
    private readonly float _yDirection = .65f;

    public void OnPointerMove(PointerEventData eventData)
    {
        if(_select == false) return;
        
        var pos = eventData.delta.normalized;
        if (pos.y >= _yDirection)
        {
            OnCubeDrag?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _select = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _select = false;
    }
}
