using System;
using System.Collections.Generic;
using Source.Scripts.ActionInfromer;
using Source.Scripts.DragAndDrop;
using Source.Scripts.Inventory;
using Source.Scripts.Save;
using UnityEngine;
using Zenject;
using Source.Scripts.Extensions;
using Source.Scripts.ObjectPools;
using Source.Scripts.Utils;

public class Tower : MonoBehaviour, IDropZone, ILoadProgress, ISaveProgress
{
    private DragObjectMover _dragObjectMover;
    private CubeInventory _inventory;
    private IPlacementStrategy _placementStrategy;
    private IStaticDataService _staticDataService;
    private ISaveLoadService _iSaveLoadService;
    private CubeAnimationService _cubeAnimationService;
    private ActionInformerService _actionInformerService;
    private CubeObjectPoolProvider _objectPoolProvider;
    
    private readonly Dictionary<Type, IPlacementStrategy> _placementStrategies = new Dictionary<Type, IPlacementStrategy>();
    
    [Inject]
    private void Initialize(RayCaster raycaster, DragObjectMover dragObjectMover, 
        IStaticDataService staticDataService, ISaveLoadService prefsSaveLoadService,
        CubeAnimationService cubeAnimationService, ActionInformerService actionInformerService, 
        CubeObjectPoolProvider objectPoolProvider)
    {
        _dragObjectMover = dragObjectMover;
        _inventory = new CubeInventory(new List<Cube>());
        _staticDataService = staticDataService;
        _iSaveLoadService = prefsSaveLoadService;
        _cubeAnimationService = cubeAnimationService;
        _actionInformerService = actionInformerService;
        _objectPoolProvider = objectPoolProvider;
    }

    private void Start()
    {
        _dragObjectMover.OnSelect += TrySelectCubeFromTower;
        
        _placementStrategies.Add(typeof(FoundationPlacement), new FoundationPlacement(this, _cubeAnimationService, _actionInformerService));
        _placementStrategies.Add(typeof(StackedPlacement), new StackedPlacement(_dragObjectMover, this, _cubeAnimationService, _actionInformerService));
        
        if (_inventory.Items.Count != 0)
        {
            SetPlacementStrategy<StackedPlacement>();
            return;
        }
        
        SetPlacementStrategy<FoundationPlacement>();
    }

    public bool IsAvailableAria(Vector3 position)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(position);
            
        float marginX = 0.03f;
        float marginY = 0.03f;
        
        var isAvailable = viewportPos.x > marginX && viewportPos.x < 1 - marginX &&
                          viewportPos.y > marginY && viewportPos.y < 1 - marginY;

        if (isAvailable == false)
        {
            _cubeAnimationService.OnExplosion(_dragObjectMover.SelectedDragAndDrop, 
                () =>
                {
                    _actionInformerService.ShowActionByTag("out_range");
                    SaveProgress();
                });
        }
        
        return isAvailable;
    }

    private void TrySelectCubeFromTower(IDragAndDrop dragAndDrop)
    {
        var cube = dragAndDrop.GetGameObject().GetComponent<Cube>();
        if (_inventory.Items.Contains(cube) == false) return;
        
        RemoveCube(cube);
        _dragObjectMover.Select(dragAndDrop);
    }

    private void RemoveCube(Cube cube)
    {
        var indexOf = _inventory.Items.IndexOf(cube);

        cube.transform.SetParent(null);
        _inventory.RemoveItem(cube);
        
       _cubeAnimationService.RedrawTower(indexOf, _inventory.Items, SaveProgress);
        
        if (_inventory.Items.Count == 0)
        {
            SetPlacementStrategy<FoundationPlacement>();
        }
    }

    public void AddCube(Cube cube)
    {
        cube.transform.SetParent(transform);
        _inventory.AddItem(cube);
    }

    public void SetPlacementStrategy<IPlacementStrategy>()
    {
        _placementStrategy = _placementStrategies[typeof(IPlacementStrategy)];
    }

    public void Drop(IDragAndDrop dragAndDrop)
    {
        _placementStrategy.Place(dragAndDrop);
    }

    public void SaveProgress()
    {
        _iSaveLoadService.SaveProgress();
    }

    public void Load(PlayerProgress playerProgress)
    {
        var colors = _staticDataService.GetInventoryColors();
        
        for (int i = 0; i <  playerProgress.TowerData.CubeData.Count; i++)
        {
            var cubeData = playerProgress.TowerData.CubeData[i];

            var newCube = _objectPoolProvider.ObjectPool.Get();
            
            newCube.transform.SetParent(transform);
            newCube.ColorID = cubeData.ColorID;
            newCube.Visual.sprite = colors[cubeData.ColorID];
            newCube.transform.localPosition = cubeData.Vector2Data.ToVector3();
            
            _inventory.AddItem(newCube);
        }
    }

    public void Save(PlayerProgress playerProgress)
    {
        playerProgress.TowerData.CubeData.Clear();
        
        for (int i = 0; i < _inventory.Items.Count; i++)
        {
            var item = _inventory.Items[i];
            Vector2Data cubePosition = item.transform.localPosition.ToVector2Data();
            CubeData cubeData = new CubeData(cubePosition, _inventory.Items[i].ColorID);
            playerProgress.TowerData.CubeData.Add(cubeData);
        }
    }

    private void OnDestroy()
    {
        _dragObjectMover.OnDeselect -= TrySelectCubeFromTower;
    }
}