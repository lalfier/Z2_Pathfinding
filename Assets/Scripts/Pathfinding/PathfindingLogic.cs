using System;
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;

[RequireComponent(typeof(PathfindingGrid))]
public class PathfindingLogic : MonoBehaviour
{
    private PathfindingGrid grid;   // Grid reference
    private HeapMin openSet;    // HeapMin of nodes that needs to be checked
    private HashSet<GridNode> closedSet;    // HashSet of nodes already checked

    private void Awake()
    {
        grid = GetComponent<PathfindingGrid>();
        openSet = new HeapMin(grid.NumberOfGridNodes);
        closedSet = new HashSet<GridNode>();
    }

    /// <summary>
    /// Calculates the path from start to end position using A* pathfinding algorithm.
    /// </summary>
    /// <param name="startPos">Start world position.</param>
    /// <param name="endPos">End world position.</param>
    /// <param name="returnAllWaypoints">Return all waypoints from path calculations or ones with direction change.</param>
    /// <returns>Returns array of path waypoints (world position).</returns>
    public Vector3[] CalculatePath(Vector3 startPos, Vector3 endPos, bool returnAllWaypoints)
    {
        // Start debug diagnostics
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        // Get grid node from position
        GridNode startNode = grid.GetNodeFromPosition(startPos);
        GridNode endNode = grid.GetNodeFromPosition(endPos);

        openSet.Clear();    // Empty HeapMin before calculation
        closedSet.Clear();  // Empty HashSet before calculation

        // Add start node to open set
        openSet.Add(startNode);

        // Loop through open set
        while (openSet.Count > 0)
        {
            // Set current node as lowest FScore node in open set
            GridNode currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);     // Add current node in closed HashSet

            // If current node is end node, algorithm is finished and path is found, success
            if (currentNode == endNode)
            {
                // Stop debug diagnostics
                //sw.Stop();
                //UnityEngine.Debug.Log("Success! Path found in: " + sw.ElapsedMilliseconds + " ms");

                return ReconstructPath(startNode, endNode, returnAllWaypoints);
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
                    else
                    {
                        openSet.UpdateNode(neighbor);
                    }
                }
            }
        }
        // Open set is empty but end node was never reached, failure
        return null;
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

    private Vector3[] ReconstructPath (GridNode startNode, GridNode endNode, bool returnAllWaypoints)
    {
        // Create path from end to start node
        List<GridNode> path = new List<GridNode>();
        GridNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        // Get waypoints(world position) from path
        Vector3[] waypoints = GetWaypoints(path, returnAllWaypoints);
        Array.Reverse(waypoints);   // Reorder list so it is from start to end node
        return waypoints;
    }

    private Vector3[] GetWaypoints(List<GridNode> path, bool returnAllWaypoints)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 previousDir = Vector2.zero;
        
        for (int i = 1; i < path.Count; i++)
        {
            if (returnAllWaypoints)
            {
                // Get world position from all path nodes
                waypoints.Add(path[i].WorldPosition);
            }
            else
            {
                // If there is change in direction record waypoint world position
                Vector2 nextDir = new Vector2((path[i - 1].GridPosX - path[i].GridPosX), (path[i - 1].GridPosY - path[i].GridPosY));
                if (nextDir != previousDir)
                {
                    waypoints.Add(path[i].WorldPosition);
                }
                previousDir = nextDir;
            }
        }

        return waypoints.ToArray();
    }
}
