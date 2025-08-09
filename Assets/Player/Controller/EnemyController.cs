using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3f;
    GridSystem gridSystem;
    PathFinding PathFinder;
    private Vector2Int CurrCords;
    
    List<GridStats> path = new List<GridStats>(); // This Stores the Path cords needed for Traversal:
    void Start()
    {
        gridSystem = FindFirstObjectByType<GridSystem>();
        PathFinder = FindFirstObjectByType<PathFinding>();
        
        Debug.Log("Pos -> " + gridSystem.gridState.PlayerPos);
    }

    void Update()
    {
        Vector2Int startCords = new Vector2Int((int) transform.position.x, (int) transform.position.z) / gridSystem.GetGridSize;
        Vector2Int targetCords = gridSystem.gridState.PlayerPos;
        if (CurrCords != targetCords)
        {
            PathFinder.SetNewTarget(startCords, targetCords);
            RecalculatePath(true);
            Debug.Log("Pos------------------ -> " + gridSystem.gridState.PlayerPos);
        }
        CurrCords = targetCords;
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
        StartCoroutine(FollowPath());
    }
    
    IEnumerator FollowPath()
    {
        // because of this forLoop we are able to Travel in the direction of the correct coordinates:
        // One coord at a time ----------->
        for (int i = 1; i < path.Count-1; i++)
        {
            Vector3 startPosition = transform.position;
            // converting Coordinates to in game position:
            Vector3 endPosition = gridSystem.GetPositionFromCoordinates(path[i].cordData);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }

        }
    }
}
