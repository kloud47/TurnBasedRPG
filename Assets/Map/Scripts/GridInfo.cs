using System;
using TMPro;
using Unity.VisualScripting;
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
        label.gameObject.SetActive(false);
        
        DisplayCoords();
    }

    private void OnMouseEnter()
    {
        // Debug.Log("Mouse entered: " + cords.ToString());
        label.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        label.gameObject.SetActive(false);
    }

    private void DisplayCoords()
    {
        if (!gridSystem) { return; }
        cords.x = Mathf.RoundToInt(transform.position.x / gridSystem.GetGridSize);
        cords.y = Mathf.RoundToInt(transform.position.z / gridSystem.GetGridSize);
        label.text = $"{cords.x}, {cords.y}";
    }
}
