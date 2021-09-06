using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindingGrid))]
public class PathfindingLogic : MonoBehaviour
{
    [Tooltip("Game object representing start")]
    public Transform startPoint;
    [Tooltip("Game object representing end")]
    public Transform endPoint;

    private PathfindingGrid grid;   // Grid reference

    private void Awake()
    {
        grid = GetComponent<PathfindingGrid>();
    }

    private void Update()
    {
        CalculatePath(startPoint.position, endPoint.position);
    }

    private void CalculatePath(Vector3 startPos, Vector3 endPos)
    {
        // Get grid node from position
        GridNode startNode = grid.GetNodeFromPosition(startPos);
        GridNode endNode = grid.GetNodeFromPosition(endPos);

        List<GridNode> openSet = new List<GridNode>();  // Set of nodes that needs to be checked
        HashSet<GridNode> closedSet = new HashSet<GridNode>();  // HashSet of nodes already checked
        // Add start node to open set
        openSet.Add(startNode);

        // Loop through open set
        while (openSet.Count > 0)
        {
            // Set current node as lowest FScore node in open set
            GridNode currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if(openSet[i].FScore < currentNode.FScore || (openSet[i].FScore == currentNode.FScore && openSet[i].HScore < currentNode.HScore))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);    // Remove current node from open set
            closedSet.Add(currentNode);     // Add current node in closed HashSet

            // If current node is end node, algorithm is finished and path is found
            if (currentNode == endNode)
            {
                ReconstructPath(startNode, endNode);
                return;
            }

            // Check all neighbors around current node
            foreach (GridNode neighbor in grid.GetNeighborNodes(currentNode))
            {
                // If neighbor is obstacle or already checked, skip to next one
                if(neighbor.IsObstacle || closedSet.Contains(neighbor))
                {
                    continue;
                }

                // Get the distance from start to the neighbor through current
                int tentativeGScore = currentNode.GScore + GetNodeDistance(currentNode, neighbor);
                if(tentativeGScore < neighbor.GScore || !openSet.Contains(neighbor))
                {
                    // This path to neighbor is better than any previous one. Record it.
                    neighbor.GScore = tentativeGScore;
                    neighbor.HScore = GetNodeDistance(neighbor, endNode);
                    neighbor.Parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    private int GetNodeDistance(GridNode startNode, GridNode endNode)
    {
        // Get vertical and horizontal distance
        int disX = Mathf.Abs(startNode.GridPosX - endNode.GridPosX);
        int disY = Mathf.Abs(startNode.GridPosY - endNode.GridPosY);

        // Check if horizontal distance is bigger
        if(disX > disY)
        {
            return (14 * disY + 10 * (disX - disY));
        }
        return (14 * disX + 10 * (disY - disX));
    }

    private void ReconstructPath (GridNode startNode, GridNode endNode)
    {
        // Create path from end to start node
        List<GridNode> path = new List<GridNode>();
        GridNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse(); // Reorder list so it is from start to end node
        grid.path = path;   // Populate path in grid for visualization
    }
}
