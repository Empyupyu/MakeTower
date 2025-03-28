using UnityEngine;

namespace Source.Scripts.Cameras
{
    public interface ICameraProvider
    {
        Camera MainCamera { get; }
    }
}