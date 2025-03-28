using Source.Scripts.ActionInfromer;
using Source.Scripts.DragAndDrop;
using Source.Scripts.Save;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Hole
{
    public class Hole : MonoBehaviour, IDropZone
    {
        [SerializeField] private Transform _ellipseCenter;
        [SerializeField] private float _semiMajorAxis= 2f;
        [SerializeField] private float _semiMinorAxis= 1f;

        private CubeAnimationService _cubeAnimationService;
        private ActionInformerService _actionInformerService;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Initialize(CubeAnimationService cubeAnimationService, ActionInformerService actionInformerService, ISaveLoadService saveLoadService)
        {
            _cubeAnimationService = cubeAnimationService;
            _actionInformerService = actionInformerService;
            _saveLoadService = saveLoadService;
        }
        
        public void Drop(IDragAndDrop dragAndDrop)
        {
            var dropObj = dragAndDrop.GetGameObject();
            var point = dropObj.transform.position;

            if (IsPointInsideEllipse(point))
            {
                _cubeAnimationService.OnDrop(dragAndDrop, () =>
                {
                    _saveLoadService.SaveProgress();
                    _actionInformerService.ShowActionByTag("rolled_cube");
                });
            }
        }

        private bool IsPointInsideEllipse(Vector3 point)
        {
            Vector2 center = _ellipseCenter.position;
            
            float xTerm = Mathf.Pow(point.x - center.x, 2) / Mathf.Pow(_semiMajorAxis, 2);
            float yTerm = Mathf.Pow(point.y - center.y, 2) / Mathf.Pow(_semiMinorAxis, 2);

            return (xTerm + yTerm) <= 1f;
        }
    }
}