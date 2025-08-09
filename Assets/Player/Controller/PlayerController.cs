using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    Transform selectedUnitTransform;
    GameObject selectedUnit;
    bool unitSelected = false;
    private bool inputEnabled = true;
    public Vector2Int cords;
    
    // UI Elements:
    public TMP_Text Coordinates;
    public TMP_Text UnitName;
    
    List<GridStats> path = new List<GridStats>(); // This Stores the Path cords needed for Traversal:
    
    GridSystem gridSystem;
    PathFinding PathFinder;
    void Start()
    {
        gridSystem = FindFirstObjectByType<GridSystem>();
        PathFinder = FindFirstObjectByType<PathFinding>();
        
        // foreach (KeyValuePair<Vector2Int, GridStats> data in gridSystem.Grid)
        // {
        //     Debug.Log(data.Key.ToString());
        // }
        gridSystem.gridState.PlayerPos =
            new Vector2Int((int)transform.position.x, (int)transform.position.z) / gridSystem.GetGridSize;
        SetCords(selectedUnitTransform);
    }
    
    void Update()
    {
        // Updating coordinates for UI:
        if (selectedUnitTransform != null)
        {
            SetCords(selectedUnitTransform);
        }
        
        // On Mouse selection:
        if (Input.GetMouseButtonDown(0) && inputEnabled)
        {
            // performing RayCast from mouse position:
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit); // check for Valid Hit:
            
            if(hasHit)
            {
                // Check for hits -> Tile or Unit:
                if(hit.transform.tag == "Tile")
                {
                    Debug.Log("Hit happened Tile");
                    if(unitSelected)
                    {
                        Vector2Int targetCords = hit.transform.GetComponent<Tile>().cords;
                        // Divide coordinates by GirdSize to get the correct Coordinate:
                        Vector2Int startCords = new Vector2Int((int) selectedUnitTransform.position.x, (int) selectedUnitTransform.position.z) / gridSystem.GetGridSize;
                        PathFinder.SetNewTarget(startCords, targetCords);// This gets the coordinates to the destination which can then be used to perform the running animation:
                        RecalculatePath(true);
                    }
                }
                if(hit.transform.tag == "Unit")
                {
                    Debug.Log("Hit happened Unit");
                    selectedUnitTransform = hit.transform;
                    selectedUnit = hit.transform.gameObject;
                    unitSelected = !unitSelected;
                }
            }
        }

        if (selectedUnit != null)
        {
            Coordinates.text = cords.x + ":" + cords.y;
            UnitName.text = selectedUnit.name;
        }
    }

    void RecalculatePath(bool recalculatePath)
    {
        Vector2Int coords = new Vector2Int();
        if (recalculatePath)
        {
            coords = PathFinder.StartPos;
        }
        else
        {
            coords = gridSystem.GetCoordinatesFromPosition(transform.position);
        }
        // Stop all running coroutines:
        StopAllCoroutines();
        path.Clear();
        path = PathFinder.GetNewPath(coords);
        // Optimization Added here using Unity Coroutines: 
        inputEnabled = false;
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        // because of this forLoop we are able to Travel in the direction of the correct coordinates:
        // One coord at a time ----------->
        for (int i = 1; i < path.Count; i++)
        {
            inputEnabled = false;
            Vector3 startPosition = selectedUnitTransform.position;
            Vector3 endPosition = gridSystem.GetPositionFromCoordinates(path[i].cordData);
            float distance = Vector3.Distance(startPosition, endPosition);
            float duration = distance / movementSpeed;
            float elapsedTime = 0f;

            // Face movement direction
            selectedUnitTransform.LookAt(endPosition);

            // Smooth movement between points
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration); // value used for Interpolation:
                selectedUnitTransform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null; // Wait for next frame
            }
            
            // Ensure exact position at end
            selectedUnitTransform.position = endPosition;
        }

        gridSystem.gridState.PlayerPos =
            new Vector2Int((int)selectedUnitTransform.position.x, (int)selectedUnitTransform.position.z) /
            gridSystem.GetGridSize;
        inputEnabled = true;
    }
    
    private void SetCords(Transform Obj)
    {
        int x = (int)Obj.position.x;
        int z = (int)Obj.position.z;

        cords = new Vector2Int(x / gridSystem.GetGridSize, z / gridSystem.GetGridSize);
    }
}
