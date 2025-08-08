using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // [SerializeField] float movementSpeed = 1f;
    Transform unitTransform;
    bool unitSelected = false;
    
    GridSystem gridSystem;
    void Start()
    {
        gridSystem = FindFirstObjectByType<GridSystem>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            
            if(hasHit)
            {
                if(hit.transform.tag == "Tile")
                {
                    Debug.Log("Hit happened Tile");
                    if(unitSelected)
                    {
                        Vector2Int targetCords = hit.transform.GetComponent<GridInfo>().cords;
                        Vector2Int startCords = new Vector2Int((int) unitTransform.position.x, (int) unitTransform.position.y) / gridSystem.GetGridSize;

                        unitTransform.transform.position = new Vector3(targetCords.x, unitTransform.position.y, targetCords.y);
                    }
                }

                if(hit.transform.tag == "Unit")
                {
                    Debug.Log("Hit happened Unit");
                    unitTransform = hit.transform;
                    unitSelected = !unitSelected;
                }
            }
        }
    }
}
