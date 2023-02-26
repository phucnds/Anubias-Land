using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2f;

    public static LevelGrid Instance { get; private set; }

    public event UnityAction OnAnyUnitMovedGridPosition;

    private GridSystem<GridObject> gridSystem;



    private void Awake()
    {
        Instance = this;
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPostition) => new GridObject(g, gridPostition));
    }

    private void Start()
    {
        Pathfindings.Instance.Setup(width, height, cellSize);
    }

    public void AddUnitAtGridPosition(GridPosition gridPostition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetListUnitAtGridPosition(GridPosition gridPostition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostition);
        return gridObject.GetListUnit();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPostition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPostion(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        OnAnyUnitMovedGridPosition?.Invoke();
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPostition) => gridSystem.IsValidGridPosition(gridPostition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public bool HasAnyUnitOnGridPosition(GridPosition gridPostition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPostition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostition);
        return gridObject.GetUnit();
    }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }
}
