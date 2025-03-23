using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utils
{
    public class ResetScrollViewPosition : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;

        void Start()
        {
            ResetScrollPosition();
        }

        void ResetScrollPosition()
        {
            if (_scrollRect == null) return;
            
            _scrollRect.horizontalNormalizedPosition = 0f;
            _scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}