using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    
    public GridStateData gridState;
    bool [,] gridstates = new bool[10,10];
    
    
    [SerializeField] int unitGridSize;
    public int GetGridSize { get { return unitGridSize; } }

    Dictionary<Vector2Int, GridStats> grid = new Dictionary<Vector2Int, GridStats>();
    public Dictionary<Vector2Int, GridStats> Grid { get { return grid; } }

    private void Awake()
    {
        // if (gridState != null) {
        //     gridstates = gridState.gridStates;
        //     // Use the data
        // }
        CreateGrid();
    }
    
    public GridStats GridCoords(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }

        return null;
    }

    public void BlockGrid(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            Debug.Log($"Blocking grid {coordinates}");
            grid[coordinates].traversable = false;
        }
    }

    public void UnBlockGrid(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            Debug.Log($"Unblocking grid {coordinates}");
            grid[coordinates].traversable = true;
        }
    }

    public void ResetGrid()
    {
        foreach (KeyValuePair<Vector2Int, GridStats> entry in grid)
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
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int cords = new Vector2Int(x, y);
                grid.Add(cords, new GridStats(cords, true));

                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //Vector3 position = new Vector3(cords.x * unitGridSize, 0f, cords.y * unitGridSize);
                //cube.transform.position = position;
                //cube.transform.SetParent(transform);
            }
        }
    }
}
