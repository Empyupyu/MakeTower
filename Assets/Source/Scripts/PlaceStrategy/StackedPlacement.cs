using Source.Scripts.ActionInfromer;
using Source.Scripts.DragAndDrop;
using Source.Scripts.Inventory;
using UnityEngine;
using Random = UnityEngine.Random;

public class StackedPlacement : IPlacementStrategy
{
    private readonly DragObjectMover _dragObjectMover;
    private readonly Tower _tower;
    private readonly CubeAnimationService _cubeAnimationService;
    private readonly ActionInformerService _actionInformerService;
    private readonly float _maxAvailableDistanceForPlace = .5f;

    public StackedPlacement(DragObjectMover dragObjectMover, Tower tower, CubeAnimationService cubeAnimationService, ActionInformerService actionInformerService)
    {
        _dragObjectMover = dragObjectMover;
        _tower = tower;
        _cubeAnimationService = cubeAnimationService;
        _actionInformerService = actionInformerService;
    }

    public void Place(IDragAndDrop dragAndDrop)
    {
        var selectedCube = dragAndDrop.GetGameObject().GetComponent<Cube>();
        var lastCubeInTower = _tower.transform.GetChild(_tower.transform.childCount - 1);

        float halfHeight = selectedCube.Visual.size.y / 2;
        Vector3 lastCubeTop = lastCubeInTower.transform.position + Vector3.up * halfHeight;
        Vector3 selectedCubeBottom = selectedCube.transform.position - Vector3.up * halfHeight;

        if (_tower.IsAvailableAria(selectedCube.transform.position) == false) return;
       
        if (Vector3.Distance(lastCubeTop, selectedCubeBottom) <= _maxAvailableDistanceForPlace)
        {
            var randomPosition = GenerateRandomPosition(lastCubeInTower, selectedCube);
            _cubeAnimationService.OnAddItemToRandomPosition(selectedCube, randomPosition, _tower.SaveProgress);
                
            _tower.AddCube(selectedCube);
            _actionInformerService.ShowActionByTag("set_cube");
            return;
        }
        
        _cubeAnimationService.OnExplosion(_dragObjectMover.SelectedDragAndDrop, () =>
        {
            _actionInformerService.ShowActionByTag("explosion_cube");
            _tower.SaveProgress();
        });
    }
    
    private Vector3 GenerateRandomPosition(Transform lastCubeInTower, Cube selectedCube)
    {
        var lastCubePosition = lastCubeInTower.transform.position;
        
        var position = new Vector3(Random.Range(
                lastCubePosition.x - (selectedCube.Visual.size.x / 2),
                lastCubePosition.x + (selectedCube.Visual.size.x / 2)),
            lastCubePosition.y + selectedCube.Visual.size.y, lastCubePosition.z);
        
        return position;
    }
}