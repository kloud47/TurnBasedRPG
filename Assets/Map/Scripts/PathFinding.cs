using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Vector2Int startPos;
    public Vector2Int StartPos { get { return startPos; } }

    [SerializeField] private Vector2Int targetPos;
    public Vector2Int TargetPos { get { return targetPos; } }

    GridStats startNode;
    GridStats targetNode;
    GridStats currentNode;
    
    Queue<GridStats> queueData = new Queue<GridStats>();
    Dictionary<Vector2Int, GridStats> arrived = new Dictionary<Vector2Int, GridStats>();
    
    GridSystem gridSystem;
    Dictionary<Vector2Int, GridStats> grid = new Dictionary<Vector2Int, GridStats>();

    private Vector2Int[] searchOrder = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    private void Awake()
    {
        gridSystem = FindFirstObjectByType<GridSystem>();
        if (gridSystem != null)
        {
            grid = gridSystem.Grid;
        }
    }
    
    public List<GridStats> GetNewPath(Vector2Int coordinates)
    {
        gridSystem.ResetGrid();

        BreadthFirstSearch(coordinates);
        return BuildPath();
    }
    
    void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.traversable = true;
        targetNode.traversable = true;

        queueData.Clear();
        arrived.Clear();

        bool isRunning = true; // Important to do the running animation:

        queueData.Enqueue(grid[coordinates]);
        arrived.Add(coordinates, grid[coordinates]);

        while (queueData.Count > 0 && isRunning == true)
        {
            currentNode = queueData.Dequeue();
            currentNode.visited = true;
            ExploreNeighbors();
            if (currentNode.cordData == TargetPos)
            {
                isRunning = false; // dont move here moving:
                currentNode.traversable = false;// important so that two player units don't stand on each other:
            }
        }
    }
    
    // code to check all nearest Neighbours:
    void ExploreNeighbors()
    {
        List<GridStats> neighbors = new List<GridStats>();

        foreach (Vector2Int direction in searchOrder)
        {
            Vector2Int neighborCords = currentNode.cordData + direction;

            if (grid.ContainsKey(neighborCords))
            {
                neighbors.Add(grid[neighborCords]);
            }
        }

        foreach (GridStats neighbor in neighbors)
        {
            if (!arrived.ContainsKey(neighbor.cordData) && neighbor.traversable)
            {
                neighbor.next = currentNode;
                arrived.Add(neighbor.cordData, neighbor);
                queueData.Enqueue(neighbor);
            }
        }
    }
    
    //  Queue Data is reversed and Stored in Path:
    List<GridStats> BuildPath()
    {
        List<GridStats> path = new List<GridStats>();
        GridStats currentNode = targetNode;

        path.Add(currentNode);
        currentNode.path = true;

        while (currentNode.next != null)
        {
            currentNode = currentNode.next;
            path.Add(currentNode);
            currentNode.path = true;
        }

        path.Reverse();
        return path;
    }

    // Whenever a tile is clicked or a new PLayer changes position this code is executed to find a new Location to move to:
    public void SetNewTarget(Vector2Int startCoordinates, Vector2Int targetCoordinates)
    {
        startPos = startCoordinates;
        targetPos = targetCoordinates;
        startNode = grid[this.startPos];
        targetNode = grid[this.TargetPos];
        GetNewPath(startPos);
    }
}
