using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public bool blocked;
    public Vector2Int cords;
    GridSystem gridManager;
    
    // Visual representation:
    private GameObject sphereIndicator;
    private const float sphereOffset = 0f;
    private const float sphereRadius = 0.4f;
    
    private static Material redMaterial;

    void Start()
    {
        SetCords();
        VisualUpdate();// generate red sphere if the blocked == true:
        if(blocked)
        {
            gridManager.BlockGrid(cords);
            
        }
    }

    private void SetCords()
    {
        gridManager = FindFirstObjectByType<GridSystem>();
        int x = (int)transform.position.x;
        int z = (int)transform.position.z;

        cords = new Vector2Int(x / gridManager.GetGridSize, z / gridManager.GetGridSize);
    }

    private void VisualUpdate()
    {
        // Remove existing indicator if it exists
        if (sphereIndicator != null)
        {
            Destroy(sphereIndicator);
        }

        if (blocked)
        {
            sphereIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphereIndicator.transform.position = transform.position + Vector3.up * sphereOffset;
            sphereIndicator.transform.localScale = Vector3.one * sphereRadius * 2;
            
            // Set Red Color:
            // Get or create the red material
            if (redMaterial == null)
            {
                redMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                redMaterial.color = Color.red;
            }
            Renderer renderer = sphereIndicator.GetComponent<Renderer>();
            renderer.material = redMaterial;
            
            // Destroying as collision is not needed:
            Destroy(sphereIndicator.GetComponent<Collider>());
            sphereIndicator.transform.SetParent(transform);
        }
    }
}
