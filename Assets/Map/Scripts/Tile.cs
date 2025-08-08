using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool blocked;

    public Vector2Int cords;

    GridSystem gridManager;

    void Start()
    {
        SetCords();

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
}
