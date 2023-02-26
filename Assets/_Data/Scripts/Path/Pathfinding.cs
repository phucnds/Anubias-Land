using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 1400;

    [SerializeField] private LayerMask obstaclesLayerMark;

    private int width;
    private int height;
    private float cellSize;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        Instance = this;

    }

    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMark))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }


    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);

        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition GridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(GridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);
            if (currentNode == endNode)
            {
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endNode.GetGridPosition()));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }

        }

        pathLength = 0;
        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();

        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> GridPositionList = new List<GridPosition>();

        foreach (PathNode pathNode in pathNodeList)
        {
            GridPositionList.Add(pathNode.GetGridPosition());
        }

        return GridPositionList;
    }

    public int CalculateDistance(GridPosition GridPositionA, GridPosition GridPositionB)
    {
        GridPosition GridPositionDistance = GridPositionA - GridPositionB;
        int xDistance = Mathf.Abs(GridPositionDistance.x);
        int zDistance = Mathf.Abs(GridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;


    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition GridPosition = currentNode.GetGridPosition();

        bool LEFT, RIGHT, UP, DOWN;

        LEFT = GridPosition.x - 1 > 0;
        RIGHT = GridPosition.x + 1 < gridSystem.GetWidth();
        UP = GridPosition.z + 1 < gridSystem.GetHeight();
        DOWN = GridPosition.z - 1 > 0;

        if (LEFT) neighbourList.Add(GetNode(GridPosition.x - 1, GridPosition.z + 0));
        if (RIGHT) neighbourList.Add(GetNode(GridPosition.x + 1, GridPosition.z + 0));
        if (UP) neighbourList.Add(GetNode(GridPosition.x + 0, GridPosition.z + 1));
        if (DOWN) neighbourList.Add(GetNode(GridPosition.x + 0, GridPosition.z - 1));

        // if (LEFT && DOWN) neighbourList.Add(GetNode(GridPosition.x - 1, GridPosition.z - 1));
        // if (LEFT && UP) neighbourList.Add(GetNode(GridPosition.x - 1, GridPosition.z + 1));
        // if (RIGHT && DOWN) neighbourList.Add(GetNode(GridPosition.x + 1, GridPosition.z - 1));
        // if (RIGHT && UP) neighbourList.Add(GetNode(GridPosition.x + 1, GridPosition.z + 1));

        return neighbourList;
    }

    public void SetWalkableGridPosition(GridPosition GridPosition, bool isWalkable)
    {
        gridSystem.GetGridObject(GridPosition).SetIsWalkable(isWalkable);
    }

    public bool IsWalkableGridPosition(GridPosition GridPosition)
    {
        return gridSystem.GetGridObject(GridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }

}
