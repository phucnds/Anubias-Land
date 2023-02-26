using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveAction : BaseAction
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float stoppingDistance = .1f;

    private int maxMoveDistance = 500;

    public event UnityAction OnStartMoving;
    public event UnityAction OnStopMoving;

    private List<Vector3> positionList;
    private int currentPositionIndex;


    private void Update()
    {
        if (!isActive) return;

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime);
        }
        else
        {
            currentPositionIndex++;

            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke();
                ActionComplete();
            }
        }
    }




    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (unitGridPosition == testGridPosition) continue;
                //if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                if (!Pathfindings.Instance.IsWalkableGridPosition(testGridPosition)) continue;
                if (!Pathfindings.Instance.HasPath(unitGridPosition, testGridPosition)) continue;

                int pathfindingDistanceMultiplier = 10;
                if (Pathfindings.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier) continue;
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfindings.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke();
        ActionStart(onActionComplete);
    }

    public bool MoveTo(Vector3 destination,Action onActionComplete)
    {
        GridPosition unitGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        GridPosition testGridPosition = LevelGrid.Instance.GetGridPosition(destination);

        if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
        {
            Debug.Log("IsValidGridPosition");
            return false;
        } 
        if (unitGridPosition == testGridPosition){
            Debug.Log("Same GridPosition");  
            return false;
        } 
        //if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) return false;
        if (!Pathfindings.Instance.IsWalkableGridPosition(testGridPosition))
        {
            Debug.Log("GridPosition is not walkable");
            return false;
        } 
        if (!Pathfindings.Instance.HasPath(unitGridPosition, testGridPosition))
        {
            Debug.Log("hasn't path");
            return false;
        }

        if (testGridPosition == null) return false;

        TakeAction(testGridPosition,onActionComplete);
        return true;
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
