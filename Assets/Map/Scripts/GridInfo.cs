using System;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class GridInfo : MonoBehaviour
{
    TextMeshPro label;
    public Vector2Int cords = new Vector2Int();
    GridSystem gridSystem;

    private void Awake()
    {
        gridSystem = FindFirstObjectByType<GridSystem>();
        label = GetComponentInChildren<TextMeshPro>();
        
        DisplayCoords();
    }

    private void Update()
    {
        DisplayCoords();
        transform.name = cords.ToString();
    }

    private void DisplayCoords()
    {
        if (!gridSystem) { return; }
        cords.x = Mathf.RoundToInt(transform.position.x / gridSystem.GetGridSize);
        cords.y = Mathf.RoundToInt(transform.position.z / gridSystem.GetGridSize);
        label.text = $"{cords.x}, {cords.y}";
    }
}
