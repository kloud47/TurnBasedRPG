using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] public Vector2Int gridSize;
    
    public GridStateData gridState;
    
    
    [SerializeField] int unitGridSize;
    public int GetGridSize { get { return unitGridSize; } }

    // Dictionary<Vector2Int, GridStats> grid = new Dictionary<Vector2Int, GridStats>();
    public Dictionary<Vector2Int, GridStats> Grid { get { return gridState.grid; } }

    private void Awake()
    {
        CreateGrid();
    }
    
    public GridStats GridCoords(Vector2Int coordinates)
    {
        if (Grid.ContainsKey(coordinates))
        {
            return Grid[coordinates];
        }

        return null;
    }

    public void BlockGrid(Vector2Int coordinates)
    {
        if (Grid.ContainsKey(coordinates))
        {
            // Debug.Log($"Blocking grid {coordinates}");
            Grid[coordinates].traversable = false;
        }
    }

    public void UnBlockGrid(Vector2Int coordinates)
    {
        if (Grid.ContainsKey(coordinates))
        {
            // Debug.Log($"Unblocking grid {coordinates}");
            Grid[coordinates].traversable = true;
        }
    }

    public void ResetGrid()
    {
        foreach (KeyValuePair<Vector2Int, GridStats> entry in Grid)
        {
            entry.Value.next = null;
            entry.Value.visited = false;
            entry.Value.path = false;
        }
    }

    // This function converts Postion data into Grid coordinates:
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();

        coordinates.x = Mathf.RoundToInt(position.x / unitGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unitGridSize);

        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();

        position.x = coordinates.x * unitGridSize;
        position.z = coordinates.y * unitGridSize;

        return position;
    }

    private void CreateGrid()
    {
        Grid.Clear();
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int cords = new Vector2Int(x, y);
                // grid.Add(cords, new GridStats(cords, !gridstates[x,y]));
                Grid.Add(cords, new GridStats(cords, true));

                // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // Vector3 position = new Vector3(cords.x * unitGridSize, 0f, cords.y * unitGridSize);
                // cube.transform.position = position;
                // cube.transform.SetParent(transform);
            }
        }
    }
}
