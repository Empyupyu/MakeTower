using System;
using System.Collections;
using System.Collections.Generic;
using Source.Scripts.Inventory;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Tower : MonoBehaviour
{
    private IInput _input;
    private RayCaster _raycaster;
    private CubeHolder _cubeHolder;
    
    public CubeInventory _inventory;
    private StateMachine _stateMachine;

    [Inject]
    private void Initialize(IInput input, RayCaster raycaster, CubeHolder cubeHolder)
    {
     _input = input;   
     _raycaster = raycaster;   
     _cubeHolder = cubeHolder;
     _inventory = new CubeInventory(new List<Cube>());
     
     _stateMachine = new StateMachine();
     _stateMachine.RegisterState(typeof(FoundationState),
         new FoundationState(_raycaster, _cubeHolder, _stateMachine, this));
     _stateMachine.RegisterState(typeof(BuildingState), new BuildingState(_raycaster, _cubeHolder));
     _stateMachine.SwitchState(typeof(FoundationState));
     
     // _raycaster.OnClickUpRaycast += OnClickUpRaycast;
    }

    // private void OnClickUpRaycast(Collider2D collider)
    // {
    //     if(_cubeHolder.SelectedCube == null) return;
    //     if(!collider.TryGetComponent<Tower>(out var tower)) return;
    //     
    //     _inventory.AddItem(_cubeHolder.SelectedCube);
    //     _cubeHolder.SelectedCube.transform.SetParent(tower.transform);
    //     _cubeHolder.SetSelectedCube(null);
    // }

    private void Update()
    {
        _stateMachine.EveryFrame();
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

public class FoundationState : IState
{
    private RayCaster _rayCaster;
    private CubeHolder _cubeHolder;
    private StateMachine _stateMachine;
    private readonly Tower _tower;

    public FoundationState(RayCaster rayCaster, CubeHolder cubeHolder, StateMachine stateMachine, Tower tower)
    {
        _rayCaster = rayCaster;
        _cubeHolder = cubeHolder;
        _stateMachine = stateMachine;
        _tower = tower;
    }

    public override void Enter()
    {
        _rayCaster.OnClickUpRaycast += OnDeselect;
    }

    private void OnDeselect(Collider2D collider)
    {
        if (_cubeHolder.SelectedCube == null) return;
        if (!collider.TryGetComponent<Tower>(out var tower)) return;

        _rayCaster.OnClickUpRaycast -= OnDeselect;

        var selectedCube = _cubeHolder.SelectedCube;
        _cubeHolder.SetSelectedCube(null);
        selectedCube.transform.SetParent(tower.transform);
       
        _tower._inventory.AddItem(_cubeHolder.SelectedCube);
        
        _stateMachine.SwitchState(typeof(BuildingState));
    }

    public override void Exit()
    {
        _rayCaster.OnClickUpRaycast -= OnDeselect;
    }

    public override void EveryFrame()
    {
    }
}

public class BuildingState : IState
{
    private RayCaster _rayCaster;
    private CubeHolder _cubeHolder;

    public BuildingState(RayCaster rayCaster, CubeHolder cubeHolder)
    {
        _rayCaster = rayCaster;
        _cubeHolder = cubeHolder;
    }

    public override void Enter()
    {
        _rayCaster.OnClickUpRaycast += OnDeselect;
    }

    private void OnDeselect(Collider2D collider)
    {
        if (_cubeHolder.SelectedCube == null) return;
        if (!collider.TryGetComponent<Tower>(out var tower)) return;

        var selectedCube = _cubeHolder.SelectedCube;
        
        
        var lastCubeInTower = tower.transform.GetChild(tower.transform.childCount - 1);
        
        var lastCubeCorrectPosition = lastCubeInTower.transform.position + new Vector3(0, (selectedCube.Visual.size.y / 2), 0);
        var selectCubeCorrectPosition = selectedCube.transform.position - new Vector3(0, (selectedCube.Visual.size.y / 2), 0);

        var distance = Vector3.Distance(lastCubeCorrectPosition, selectCubeCorrectPosition);
        
        if ( distance <= .5f)
        {
            _cubeHolder.SetSelectedCube(null);
            
            selectedCube.transform.position = new Vector3(Random.Range(
                    lastCubeInTower.position.x - (selectedCube.Visual.size.x / 2),
                    lastCubeInTower.position.x + (selectedCube.Visual.size.x / 2)),
                lastCubeInTower.position.y + selectedCube.Visual.size.y, lastCubeInTower.position.z);
        
            selectedCube.transform.SetParent(tower.transform);
        }
        else
        {
          GameObject.Destroy(_cubeHolder.SelectedCube.gameObject);   
        }
    }

    public override void Exit()
    {
        _rayCaster.OnClickUpRaycast -= OnDeselect;
    }

    public override void EveryFrame()
    {
    }
}

public class RayCaster : IDisposable
{
    public Action<Collider2D> OnClickDownRaycast;
    public Action<Collider2D> OnClickUpRaycast;
    private RaycastHit2D _hit;
    private IInput _input;
    private Camera _camera;

    public RayCaster(IInput input)
    {
        _camera = Camera.main;
        _input = input;
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

public interface IInput
{
    public event Action<Vector2> ClickDown;
    public event Action<Vector2> ClickUp;
    public event Action<Vector2> Drag;
    public event Action<Vector2> Delta;

    public void InputListen();
}

public class MobileInput : IInput
{
    public event Action<Vector2> ClickDown;
    public event Action<Vector2> ClickUp;
    public event Action<Vector2> Drag;
    public event Action<Vector2> Delta;
    
    private Vector2 _lastPosition;
    
    public void InputListen()
    {
        if (Input.touchCount == 0) return;

        var touch = Input.touches[0];
        _lastPosition = (Vector2)touch.position;
   
        Delta.Invoke(touch.position - _lastPosition);

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
    
    public void InputListen()
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

public class CubeHolder
{
    public Vector3 DragOffset { get; private set; }
    public Cube SelectedCube { get; private set; }

    private IInput _input;
    
    public CubeHolder(IInput input)
    {
        _input = input;
        _input.Drag += UpdatePosition;
    }
    
    public void SetSelectedCube(Cube cube)
    {
        if(cube == null)
        {
            SelectedCube.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            cube.GetComponent<Collider2D>().enabled = false;
        }
        
        SelectedCube = cube;
    }

    public void ClearHolder()
    {
        if (SelectedCube != null)
            GameObject.Destroy(SelectedCube.gameObject);

        SetSelectedCube(null);
    }

    private void UpdatePosition(Vector2 inputPosition)
    {
        if (SelectedCube == null) return;

        var worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        worldPoint.z = SelectedCube.transform.position.z;
        SelectedCube.transform.position = worldPoint + DragOffset;
    }
}