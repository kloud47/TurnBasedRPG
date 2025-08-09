using UnityEngine;

public class GridStats
{
    public Vector2Int cordData;
    public bool traversable;
    // visited and path are used only during path finding Algo BFS:
    public bool visited;
    public bool path;
    //---------------------------------------------------------------
    public GridStats next;

    public GridStats(Vector2Int cords, bool traversable)
    {
        this.cordData = cords;
        this.traversable = traversable;
    }
}
