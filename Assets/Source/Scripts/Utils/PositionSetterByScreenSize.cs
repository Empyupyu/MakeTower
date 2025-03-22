using UnityEngine;

namespace Utils
{
    public class PositionSetterByScreenSize : MonoBehaviour
    {
        [SerializeField] private float _widthStartPoint;
        [SerializeField] private float _heightStartPoint;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        void Start()
        {
            AdjustPosition();
        }

        public void AdjustPosition()
        {
            Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            transform.position = new Vector3((screenBounds.x * _widthStartPoint + _spriteRenderer.bounds.size.x / 2),
                (screenBounds.y * _heightStartPoint + _spriteRenderer.bounds.size.y / 2),
                transform.position.z);
        }
    }
}