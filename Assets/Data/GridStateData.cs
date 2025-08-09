using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridStateData", menuName = "Scriptable Objects/GridStateData")]
public class GridStateData : ScriptableObject
{
    // public bool[,] gridStates = new bool[10,10];
    
    // without this scriptable Object You won't be able to change the obstacles at runtime:
    public Dictionary<Vector2Int, GridStats> grid = new Dictionary<Vector2Int, GridStats>();

    // Need by EnemyController to find and move our nearest location:
    public Vector2Int PlayerPos;
}
