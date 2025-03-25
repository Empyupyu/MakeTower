using System;
using System.Collections.Generic;
using DG.Tweening;
using Source.Scripts.CameraShaker;
using Source.Scripts.Interface;
using Source.Scripts.Inventory;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Tower : MonoBehaviour, IDropZone
{
    [SerializeField] private float _shakeDuration = .3f;
    [SerializeField] private Vector3 _strength;
    [SerializeField] private int _vibrato;
    [SerializeField] private Ease _ease;
    [SerializeField] private float _randomness;
    [SerializeField] private float _moveDownDuration = .3f;
    
    private IInput _input;
    private CubeMover _cubeMover;
    private CubeInventory _inventory;
    private IPlacementStrategy _placementStrategy;
    private RaycastHit2D[] _hits;
    private CameraShaker _cameraShaker;

    [Inject]
    private void Initialize(IInput input, RayCaster raycaster, CubeMover cubeMover, CameraShaker cameraShaker)
    {
        _input = input;
        _cubeMover = cubeMover;
        _inventory = new CubeInventory(new List<Cube>());
        _hits = new RaycastHit2D[2];
        _cameraShaker = cameraShaker;
        _input.ClickDown += OnSelect;
        
        _placementStrategy = new FoundationPlacement(_cubeMover, this, _cameraShaker);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        _input.ClickDown -= OnSelect;
    }

    public void RemoveCube(Cube cube)
    {
        var indexOf = _inventory.Items.IndexOf(cube);

        cube.transform.SetParent(null);
        _inventory.RemoveItem(cube);

        Redraw(indexOf);
    }

    public void AddCube(Cube cube)
    {
        cube.transform.SetParent(transform);
        _inventory.AddItem(cube);
    }

    private void Redraw(int index)
    {
        for (int i = index; i < _inventory.Items.Count; i++)
        {
            var item = _inventory.Items[i];
       
            item.transform.DOMoveY(item.transform.position.y - item.Visual.size.y, _moveDownDuration)
                .OnComplete(() => _cameraShaker.Shake(_shakeDuration, _strength, _vibrato, _randomness, _ease));
        }
    }
    
    private void OnSelect(Vector2 position)
    {
        if (_cubeMover.SelectedCube != null) return;

        var ray = Camera.main.ScreenPointToRay(position);
        Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _hits);
        bool hasTower = false;
        bool hasCube = false;

        if (_hits.Length != 2) return;

        Cube cube = null;

        for (int i = 0; i < _hits.Length; i++)
        {
            var hit = _hits[i];

            if (hit.collider == null) break;

            if (hit.collider.TryGetComponent<Tower>(out var tower))
            {
                hasTower = true;
            }
            else if (hit.collider.TryGetComponent<Cube>(out cube))
            {
                if (cube.InWorld == true)
                {
                    hasCube = true;
                }
            }
        }

        if (hasTower == false || hasCube == false) return;

        RemoveCube(cube);
        
        
        _cubeMover.SetSelectedCube(cube);

        if (transform.childCount == 0)
        {
            SetPlacementStrategy(new FoundationPlacement( _cubeMover, this, _cameraShaker));
        }
    }
    
    public void SetPlacementStrategy(IPlacementStrategy strategy)
    {
        _placementStrategy = strategy;
    }

    public void Drop(IDrop drop)
    {
        _placementStrategy.Place(drop);
    }
}

public class StateMachine
{
    private Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
    private IState _currentState;

    public void RegisterState(Type type, IState state)
    {
        _states.Add(type, state);
    }

    public void SwitchState(Type type)
    {
        _currentState?.Exit();

        _currentState = _states[type];
        _currentState.Enter();
    }

    public void EveryFrame()
    {
        _currentState?.EveryFrame();
    }
}

public abstract class IState
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void EveryFrame();
}

public interface IPlacementStrategy
{
    public void Place(IDrop drop);
}

public class FoundationPlacement : IPlacementStrategy
{
    private readonly CubeMover _cubeMover;
    private readonly Tower _tower;
    private readonly CameraShaker _cameraShaker;

    public FoundationPlacement(CubeMover cubeMover, Tower tower, CameraShaker cameraShaker)
    {
        _cubeMover = cubeMover;
        _tower = tower;
        _cameraShaker = cameraShaker;
    }
    
    public void Place(IDrop drop)
    {
        var selectedCube = drop.GetGameObject().GetComponent<Cube>();
      
        _cubeMover.SetSelectedCube(null);
        _tower.AddCube(selectedCube);
        _tower.SetPlacementStrategy(new StackedPlacement(_cubeMover, _tower, _cameraShaker));
    }
}

public class StackedPlacement : IPlacementStrategy
{
    private readonly CubeMover _cubeMover;
    private readonly Tower _tower;
    private readonly CameraShaker _cameraShaker;
    
    //TODO
    private float _shakeDuration = .1f;
    private Vector3 _strength = new Vector3(0.05f, 0.05f, 0);
    private int _vibrato = 5;
    private float _randomness = 10;
    private Ease _ease;

    public StackedPlacement(CubeMover cubeMover, Tower tower, CameraShaker cameraShaker)
    {
        _cubeMover = cubeMover;
        _tower = tower;
        _cameraShaker = cameraShaker;
    }
    
    public void Place(IDrop drop)
    {
        var selectedCube = drop.GetGameObject().GetComponent<Cube>();
        var lastCubeInTower = _tower.transform.GetChild(_tower.transform.childCount - 1);

        var lastCubeCorrectPosition =
            lastCubeInTower.transform.position + new Vector3(0, (selectedCube.Visual.size.y / 2), 0);
        var selectCubeCorrectPosition =
            selectedCube.transform.position - new Vector3(0, (selectedCube.Visual.size.y / 2), 0);

        var distance = Vector3.Distance(lastCubeCorrectPosition, selectCubeCorrectPosition);

        if (distance <= .5f)
        {
            _cubeMover.SetSelectedCube(null);

            var position = new Vector3(Random.Range(
                    lastCubeInTower.position.x - (selectedCube.Visual.size.x / 2),
                    lastCubeInTower.position.x + (selectedCube.Visual.size.x / 2)),
                lastCubeInTower.position.y + selectedCube.Visual.size.y, lastCubeInTower.position.z);

            selectedCube.transform.DOJump(position, .3f, 1, .3f).OnComplete(() =>
            {
                _cameraShaker.Shake(_shakeDuration / 2f, _strength / 2, _vibrato / 2, _randomness / 2, _ease);
            });
            
            _tower.AddCube(selectedCube);
        }
        else
        {
            var cube = _cubeMover.SelectedCube;
            _cubeMover.SetSelectedCube(null);

            cube.transform.DOScale(0, .3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _cameraShaker.Shake(_shakeDuration, _strength, _vibrato, _randomness, _ease);
                GameObject.Destroy(_cubeMover.SelectedCube.gameObject);
            });
        }
    }
}



public class RayCaster : IDisposable
{
    public Action<Collider2D> OnClickDownRaycast;
    public Action<Collider2D> OnClickUpRaycast;
    private RaycastHit2D _hit;
    private IInput _input;
    private Camera _camera;
    private CubeMover _cubeMover;

    public RayCaster(IInput input, CubeMover cubeMover)
    {
        _camera = Camera.main;
        _input = input;
        _cubeMover = cubeMover;
        
        _input.ClickDown += OnClickDown;
        _input.ClickUp += OnClickUp;
    }

    public void Dispose()
    {
        _input.ClickDown -= OnClickDown;
        _input.ClickUp -= OnClickUp;
    }

    private void OnClickDown(Vector2 position)
    {
        _hit = RaycastHit2D(position);

        if (_hit.collider != null)
            OnClickDownRaycast?.Invoke(_hit.collider);
    }

    private void OnClickUp(Vector2 position)
    {
        _hit = RaycastHit2D(position);

        if (_hit.collider != null)
            OnClickUpRaycast?.Invoke(_hit.collider);
    }

    private RaycastHit2D RaycastHit2D(Vector3 position)
    {
        var ray = _camera.ScreenPointToRay(position);
        var hit = Physics2D.Raycast(ray.origin, ray.direction);
        return hit;
    }
}

public interface IInput : ITickable
{
    public event Action<Vector2> ClickDown;
    public event Action<Vector2> ClickUp;
    public event Action<Vector2> Drag;
    public event Action<Vector2> Delta;
}

public class MobileInput : IInput
{
    public event Action<Vector2> ClickDown;
    public event Action<Vector2> ClickUp;
    public event Action<Vector2> Drag;
    public event Action<Vector2> Delta;

    private Vector2 _lastPosition;

    public void Tick()
    {
        if (Input.touchCount == 0) return;

        var touch = Input.touches[0];
        _lastPosition = (Vector2)touch.position;

        Delta?.Invoke(touch.position - _lastPosition);

        if (touch.phase == TouchPhase.Began)
        {
            Drag?.Invoke(touch.position);
        }

        if (touch.phase == TouchPhase.Moved)
        {
            Drag?.Invoke(touch.position);
        }

        if (touch.phase == TouchPhase.Ended)
        {
            ClickUp?.Invoke(touch.position);
        }
    }
}

public class PCInput : IInput
{
    public event Action<Vector2> ClickDown;
    public event Action<Vector2> ClickUp;
    public event Action<Vector2> Drag;
    public event Action<Vector2> Delta;

    private Vector2 _lastPosition;

    public void Tick()
    {
        var click = (Vector2)Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            _lastPosition = click;
            ClickDown?.Invoke(click);
        }

        if (Input.GetMouseButton(0))
        {
            Drag?.Invoke(click);
            Delta?.Invoke(click - _lastPosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            ClickUp?.Invoke(click);
        }

        _lastPosition = click;
    }
}

public class CubeMover : IDisposable
{
    public Vector3 DragOffset { get; private set; }
    public Cube SelectedCube { get; private set; }

    private IInput _input;

    public CubeMover(IInput input)
    {
        _input = input;
        _input.Drag += UpdatePosition;
    }
    
    public void SetSelectedCube(Cube cube)
    {
        if (cube == null)
        {
            EnableCollider(SelectedCube, true);
        }
        else
        {
            EnableCollider(cube, false);
        }

        SelectedCube = cube;
    }

    private void EnableCollider(Cube cube, bool enabled)
    {
        cube.GetComponent<Collider2D>().enabled = enabled;
    }
    
    private void UpdatePosition(Vector2 inputPosition)
    {
        if (SelectedCube == null) return;

        var worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        worldPoint.z = SelectedCube.transform.position.z;
        SelectedCube.transform.position = worldPoint + DragOffset;
    }

    public void Dispose()
    {
        _input.Drag -= UpdatePosition;
    }
}