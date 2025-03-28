using System;
using System.Collections.Generic;
using DG.Tweening;
using Source.Scripts.DragAndDrop;
using Source.Scripts.Inventory;
using Source.Scripts.Utils;
using UnityEngine;

public class CubeAnimationService
{
    private readonly CubeAnimationSettings _settings;
    private readonly CameraShaker _cameraShaker;

    public CubeAnimationService(CubeAnimationSettings settings, CameraShaker cameraShaker)
    {
        _settings = settings;
        _cameraShaker = cameraShaker;
    }

    public void OnDrop(IDragAndDrop dragAndDrop, Action onDropped = null)
    {
        var dropObj = dragAndDrop.GetGameObject();
        dropObj.transform.DOMoveY(dropObj.transform.position.y - _settings.DropToHoleAnimationSettings.OffsetY,
            _settings.DropToHoleAnimationSettings.Duration);
        dropObj.transform.DOScale(0, _settings.DropToHoleAnimationSettings.Duration).OnComplete(() =>
        {
            dropObj.gameObject.SetActive(false);
            onDropped?.Invoke();
        });

        _cameraShaker.Shake(_settings.DropToHoleAnimationSettings.ShakeDuration,
            _settings.DropToHoleAnimationSettings.Strength, _settings.DropToHoleAnimationSettings.Vibrato,
            _settings.DropToHoleAnimationSettings.Randomness, _settings.DropToHoleAnimationSettings.Ease);
    }

    public void OnAddItemToRandomPosition(Cube cube, Vector3 position, Action onAdded = null)
    {
        cube.transform.DOKill();
        cube.transform.DOScale(Vector3.one, _settings.AddToTowerAnimationSettings.Duration);
        cube.transform.DOJump(position, _settings.AddToTowerAnimationSettings.JumpPower,
            _settings.AddToTowerAnimationSettings.NumJumps,
            _settings.AddToTowerAnimationSettings.Duration).OnComplete(() =>
        {
            onAdded.Invoke();

            _cameraShaker.Shake(_settings.AddToTowerAnimationSettings.ShakeDuration,
                _settings.AddToTowerAnimationSettings.Strength, _settings.AddToTowerAnimationSettings.Vibrato,
                _settings.AddToTowerAnimationSettings.Randomness, _settings.AddToTowerAnimationSettings.Ease);
        });
    }

    public void OnExplosion(IDragAndDrop dragAndDrop, Action onExploded = null)
    {
        _cameraShaker.Shake(_settings.ExplosionAnimationSettings.ShakeDuration,
            _settings.ExplosionAnimationSettings.Strength, _settings.ExplosionAnimationSettings.Vibrato,
            _settings.ExplosionAnimationSettings.Randomness, _settings.ExplosionAnimationSettings.Ease);

        var cubeTransform = dragAndDrop.GetGameObject().transform;
        cubeTransform.DORewind();

        cubeTransform.DOScale(0, _settings.ExplosionAnimationSettings.ScalingDuration).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                cubeTransform.gameObject.SetActive(false); 
                onExploded?.Invoke();
            });
    }

    public void RedrawTower(int index, List<Cube> items, Action onRedraw = null)
    {
        for (int i = index; i < items.Count; i++)
        {
            var item = items[i];

            item.transform.DOKill();
            item.transform.DOMoveY(item.transform.position.y - item.Visual.size.y,
                    _settings.RemoveFromTowerAnimationSettings.Duration)
                .OnComplete(() =>
                {
                    onRedraw?.Invoke();

                    _cameraShaker.Shake(_settings.RemoveFromTowerAnimationSettings.ShakeDuration,
                        _settings.RemoveFromTowerAnimationSettings.Strength,
                        _settings.RemoveFromTowerAnimationSettings.Vibrato,
                        _settings.RemoveFromTowerAnimationSettings.Randomness,
                        _settings.RemoveFromTowerAnimationSettings.Ease);
                });
        }
    }

    public void OnAddToTower(Cube cube, Action onAdded = null)
    {
        onAdded?.Invoke();
        
        _cameraShaker.Shake(_settings.AddToTowerAnimationSettings.ShakeDuration,
            _settings.AddToTowerAnimationSettings.Strength, _settings.AddToTowerAnimationSettings.Vibrato,
            _settings.AddToTowerAnimationSettings.Randomness, _settings.AddToTowerAnimationSettings.Ease);
    }
}