using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    [Tooltip("Draw gizmos for grid in editor?")]
    public bool showGridGizmos = true;
    [Tooltip("Size of a grid (x, y)")]
    public Vector2 gridSize;
    [Tooltip("Size of a grid node (grid unit)")]
    public float nodeSize = 1;
    [Tooltip("Obstacle layer mask for grid to distinguish")]
    public LayerMask obstacleLayerMask;

    private GridNode[,] grid;   // Two dimensional matrix of nodes that represents the grid
    private int nodesInRowX, nodesInColumnY;    // Number of gird nodes in row/column

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
                grid[x, y] = new GridNode(isObstacle, worldPoint);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Draw grid bounding box
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, gridSize);

        if (grid != null && showGridGizmos)
        {
            // Draw every grid node white, if node is clear, or red, if node is obstacle
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
