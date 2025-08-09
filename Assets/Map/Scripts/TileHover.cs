using UnityEngine;

public class TileHover : MonoBehaviour
{
    // Material Data:
    public Material TileMat;
    public Material TileMat_Overlay;
    private Renderer tileRenderer;
    
    void Start()
    {
        tileRenderer = GetComponent<Renderer>();
    }
    
    private void OnMouseEnter()
    {
        tileRenderer.material = TileMat_Overlay;
    }

    private void OnMouseExit()
    {
        tileRenderer.material = TileMat;
    }
}
