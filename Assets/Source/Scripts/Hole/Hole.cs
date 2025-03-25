using System;
using DG.Tweening;
using Source.Scripts.Interface;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Hole
{
    public class Hole : MonoBehaviour, IDropZone
    {
        public Transform ellipseCenter;
        public float semiMajorAxis = 2f;
        public float semiMinorAxis = 1f;
        private CameraShaker.CameraShaker _cameraShaker;
        [SerializeField] private float _shakeDuration = .3f;
        [SerializeField] private Vector3 _strength;
        [SerializeField] private int _vibrato;
        [SerializeField] private Ease _ease;
        [SerializeField] private float _randomness;
        [SerializeField] private float _moveDownDuration = .3f;
        
        private void Initialize(CameraShaker.CameraShaker cameraShaker)
        {
            _cameraShaker = cameraShaker;
        }
        
        private void Start()
        {
            Debug.Log(GetComponent<SpriteRenderer>().size);
        }

        public void Drop(IDrop drop)
        {
            var dropObj = drop.GetGameObject();
            var point = dropObj.transform.position;

            Vector2 center = ellipseCenter.position;

            float xTerm = Mathf.Pow((point.x - center.x), 2) / Mathf.Pow(semiMajorAxis, 2);
            float yTerm = Mathf.Pow((point.y - center.y), 2) / Mathf.Pow(semiMinorAxis, 2);

            Debug.Log((xTerm + yTerm) <= 1f);

            if ((xTerm + yTerm) <= 1f)
            {
                dropObj.transform.DOMoveY(dropObj.transform.position.y - 1, .4f);
                dropObj.transform.DOScale(0, .4f).OnComplete(() =>
                {
                    // _cameraShaker.Shake(_shakeDuration, _strength, _vibrato, _randomness, _ease);
                    Destroy(dropObj.gameObject);
                });
            }
        }
    }
}