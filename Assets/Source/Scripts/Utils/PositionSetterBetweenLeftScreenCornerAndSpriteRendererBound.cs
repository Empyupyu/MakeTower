using UnityEngine;

namespace Source.Scripts.Utils
{
    public class PositionSetterBetweenLeftScreenCornerAndSpriteRendererBound : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private SpriteRenderer _targetRenderer;
        [SerializeField] private SpriteRenderer _ownerRenderer;
        
        private readonly Vector3 _screenPoint = new Vector3(0.0f, 0.5f, 0);

        private void Start()
        {
            AdjustPosition();
        }

        public void AdjustPosition()
        {
            Vector3 _xMaxPosition = _targetRenderer.bounds.max;
            Vector3 screenPoint = Camera.main.ViewportToWorldPoint(_screenPoint);
  
            screenPoint.x = screenPoint.x >= -_ownerRenderer.bounds.size.x ? -_ownerRenderer.bounds.size.x : screenPoint.x;
                
            Vector3 centerBetweenTwoPoints = (screenPoint + _xMaxPosition) / 2;
            centerBetweenTwoPoints += _offset;
            centerBetweenTwoPoints.z = transform.position.z;

            centerBetweenTwoPoints.x = Mathf.Clamp(centerBetweenTwoPoints.x, screenPoint.x, _xMaxPosition.x - _ownerRenderer.bounds.size.x / 2);
            transform.position = centerBetweenTwoPoints;
        }
    }
}