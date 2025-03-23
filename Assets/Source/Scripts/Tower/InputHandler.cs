using System;
using UnityEngine;
using Zenject;

public class InputHandler : MonoBehaviour
{
    private IInput _input;

    [Inject]
    private void Initialize(IInput input)
    {
        _input = input;
    }

    private void Update()
    {
        _input.InputListen();
    }
}