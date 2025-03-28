using Source.Scripts.ActionInfromer;
using Source.Scripts.DragAndDrop;
using Source.Scripts.Inventory;

public class FoundationPlacement : IPlacementStrategy
{
    private readonly Tower _tower;
    private readonly CubeAnimationService _cubeAnimationService;
    private readonly ActionInformerService _actionInformerService;

    public FoundationPlacement(Tower tower, CubeAnimationService cubeAnimationService, ActionInformerService actionInformerService)
    {
        _tower = tower;
        _cubeAnimationService = cubeAnimationService;
        _actionInformerService = actionInformerService;
    }
    
    public void Place(IDragAndDrop dragAndDrop)
    {
        var selectedCube = dragAndDrop.GetGameObject().GetComponent<Cube>();
        
        if(_tower.IsAvailableAria(selectedCube.transform.position) == false) return;
        
        _tower.AddCube(selectedCube);
        _tower.SetPlacementStrategy<StackedPlacement>();
        _cubeAnimationService.OnAddToTower(selectedCube, _tower.SaveProgress);
        _actionInformerService.ShowActionByTag("set_cube");
    }
}