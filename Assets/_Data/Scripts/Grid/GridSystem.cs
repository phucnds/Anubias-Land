using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridObjectArray;

    public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }


    }

    public Vector3 GetWorldPosition(GridPosition gridPostition)
    {
        return new Vector3(gridPostition.x, 0, gridPostition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldposition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldposition.x / cellSize),
            Mathf.RoundToInt(worldposition.z / cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugPrefab, Transform parent)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPostition = new GridPosition(x, z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPostition), Quaternion.identity, parent);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPostition));

            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPostition)
    {
        return gridObjectArray[gridPostition.x, gridPostition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPostition)
    {
        return gridPostition.x >= 0 &&
               gridPostition.z >= 0 &&
               gridPostition.x < width &&
               gridPostition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}
