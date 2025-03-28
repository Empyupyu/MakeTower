using UnityEngine;

namespace Source.Scripts.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2Data ToVector2Data(this Vector3 vector)
        {
            return new Vector2Data
            {
                X = vector.x,
                Y = vector.y
            };
        }
        
        public static Vector3 ToVector3(this Vector2Data vector2Data)
        {
            return new Vector3
            {
                x = vector2Data.X,
                y = vector2Data.Y,
                z = 0
            };
        }
    }
}