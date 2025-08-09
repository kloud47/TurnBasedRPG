using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridStateData", menuName = "Scriptable Objects/GridStateData")]
public class GridStateData : ScriptableObject
{
    // public bool[,] gridStates = new bool[10,10];
    public Dictionary<Vector2Int, GridStats> grid = new Dictionary<Vector2Int, GridStats>();

    public Vector2Int PlayerPos;
}
