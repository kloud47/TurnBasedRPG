using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class GridInfo : MonoBehaviour
{
    [SerializeField] private TextMeshPro label;
    public Vector2Int cords = new();
    GridSystem gridSystem;
    
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material highlightMaterial;
    private Material originalMaterial;

    private void Awake()
    {
        gridSystem = FindFirstObjectByType<GridSystem>();
        
        if (label)
        {
            label.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"No TextMeshPro component found as child of {gameObject.name}");
        }
        
        if (meshRenderer)
        {
            originalMaterial = meshRenderer.material;
        }
        else
        {
            Debug.LogWarning($"No MeshRenderer component found on {gameObject.name}");
        }
        
        DisplayCoords();
    }

    private void OnMouseEnter()
    {
        // Debug.Log("Mouse entered: " + cords.ToString());
        if (label)
        {
            label.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Label -> " + gameObject.name);
        }
        
        if (meshRenderer && selectedMaterial)
        {
            meshRenderer.material = selectedMaterial;
        }
    }

    private void OnMouseExit()
    {
        if (label)
        {
            label.gameObject.SetActive(false);
        }
        
        if (meshRenderer != null && originalMaterial != null)
        {
            meshRenderer.material = originalMaterial;
        }
    }

    private void DisplayCoords()
    {
        if (!gridSystem) { return; }
        cords.x = Mathf.RoundToInt(transform.position.x / gridSystem.GetGridSize);
        cords.y = Mathf.RoundToInt(transform.position.z / gridSystem.GetGridSize);
        
        if (label)
        {
            label.text = $"{cords.x}, {cords.y}";
        }
    }

    public void SetAsDestination(bool flag) => meshRenderer.material = flag ? highlightMaterial : originalMaterial;
}