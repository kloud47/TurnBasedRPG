using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    Transform selectedUnitTransform;
    bool unitSelected = false;
    private bool inputEnabled = true;
    
    List<GridStats> path = new List<GridStats>(); // This Stores the Path coords needed for Traversal:
    
    GridSystem gridSystem;
    PathFinding PathFinder;
    void Start()
    {
        gridSystem = FindFirstObjectByType<GridSystem>();
        PathFinder = FindFirstObjectByType<PathFinding>();
        
        foreach (KeyValuePair<Vector2Int, GridStats> data in gridSystem.Grid)
        {
            Debug.Log(data.Key.ToString());
        }
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inputEnabled)
        {
            // performing RayCast from mouse position:
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit); // check for Valid Hit:
            
            if(hasHit)
            {
                if(hit.transform.tag == "Tile")
                {
                    Debug.Log("Hit happened Tile");
                    if(unitSelected)
                    {
                        Vector2Int targetCords = hit.transform.GetComponent<Tile>().cords;
                        // Divide coordinates by GirdSize to get the correct Coordinate:
                        Vector2Int startCords = new Vector2Int((int) selectedUnitTransform.position.x, (int) selectedUnitTransform.position.y) / gridSystem.GetGridSize;
                        PathFinder.SetNewTarget(startCords, targetCords);// This gets the coordinates to the destination which can then be used to perform the running animation:
                        RecalculatePath(true);
                    }
                }
                if(hit.transform.tag == "Unit")
                {
                    Debug.Log("Hit happened Unit");
                    selectedUnitTransform = hit.transform;
                    unitSelected = !unitSelected;
                }
            }
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
            float travelPercent = 0f;

            selectedUnitTransform.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                selectedUnitTransform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }

        }
        inputEnabled = true;
    }
}
