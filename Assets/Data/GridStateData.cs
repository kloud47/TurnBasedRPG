using UnityEngine;

[CreateAssetMenu(fileName = "GridStateData", menuName = "Scriptable Objects/GridStateData")]
public class GridStateData : ScriptableObject
{
    public bool[,] gridStates = new bool[10,10];
}
