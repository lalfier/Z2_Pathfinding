using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    [Tooltip("Draw only gizmos for path visualization")]
    public bool drawGridGizmos = false;
    [Tooltip("Size of a grid (x, y)")]
    public Vector2 gridSize;
    [Tooltip("Size of a grid node (grid unit)")]
    public float nodeSize = 1;
    [Tooltip("Obstacle layer mask for grid to distinguish")]
    public LayerMask obstacleLayerMask;

    private GridNode[,] grid;   // Two dimensional matrix of nodes that represents the grid
    private int nodesInRowX, nodesInColumnY;    // Number of gird nodes in row/column

    public int NumberOfGridNodes { get { return nodesInRowX * nodesInColumnY; } }   // Get number of grid nodes inside grid

    private void Awake()
    {
        // Get number of nodes in row/column
        nodesInRowX = Mathf.RoundToInt(gridSize.x / nodeSize);
        nodesInColumnY = Mathf.RoundToInt(gridSize.y / nodeSize);
        
        CreateGrid();
    }

    private void CreateGrid()
    {
        // Create two dimensional matrix for grid
        grid = new GridNode[nodesInRowX, nodesInColumnY];
        // Get world position of bottom left grid point
        Vector3 bottomLeftWorld = transform.position - (Vector3.right * gridSize.x / 2) - (Vector3.up * gridSize.y / 2);

        // For every node in grid
        for (int x = 0; x < nodesInRowX; x++)
        {
            for (int y = 0; y < nodesInColumnY; y++)
            {
                // Calculate node world position (from bottom left point)
                Vector3 worldPoint = bottomLeftWorld + (Vector3.right * (x * nodeSize + nodeSize / 2)) + (Vector3.up * (y * nodeSize + nodeSize / 2));
                // Check does that position is overlapping obstacle (a bit smaller radius)
                bool isObstacle = Physics2D.OverlapCircle(worldPoint, (nodeSize/2 - 0.025f), obstacleLayerMask);

                // Create grid node object
                grid[x, y] = new GridNode(isObstacle, worldPoint, x, y);
            }
        }
    }

    /// <summary>
    /// Get grid node object from world position.
    /// </summary>
    /// <param name="worldPos">World position.</param>
    /// <returns>Returns grid node object.</returns>
    public GridNode GetNodeFromPosition(Vector3 worldPos)
    {
        // If far left/down percent is 0, if far right/up percent is 1
        float percentX = worldPos.x / gridSize.x + 0.5f;
        float percentY = worldPos.y / gridSize.y + 0.5f;
        // Clamp it between 0 and 1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Calculate index x and y to locate node in a grid array
        int x = Mathf.FloorToInt(Mathf.Min(nodesInRowX * percentX, nodesInRowX - 1));
        int y = Mathf.FloorToInt(Mathf.Min(nodesInColumnY * percentY, nodesInColumnY - 1));
        return grid[x, y];
    }

    /// <summary>
    /// Get list of neighbor grid nodes.
    /// </summary>
    /// <param name="node">Grid node that looks for neighbors.</param>
    /// <returns>Returns list of neighbor grid nodes.</returns>
    public List<GridNode> GetNeighborNodes(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();
        // Get 3x3 neighbor nodes
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // This is central node so skip it
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.GridPosX + x;
                int checkY = node.GridPosY + y;
                // If node is inside grid add it to the list
                if(checkX >= 0 && checkX < nodesInRowX && checkY >= 0 && checkY < nodesInColumnY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbors;
    }

    private void OnDrawGizmos()
    {
        // Draw grid bounding box
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, gridSize);

        if (grid != null && drawGridGizmos)
        {
            // Color every grid node white, if node is clear, or red, if node is obstacle
            foreach (GridNode gNode in grid)
            {
                Gizmos.color = Color.white;
                if (gNode.IsObstacle)
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawCube(gNode.WorldPosition, Vector2.one * (nodeSize - 0.1f));  // Draw grid a bit smaller for visibility
            }
        }      
    }
}
