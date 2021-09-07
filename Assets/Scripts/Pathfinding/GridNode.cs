using UnityEngine;

public class GridNode
{
    public bool IsObstacle { get;  set; }   // Get and set is grid node an obstacle
    public Vector3 WorldPosition { get; set; }  // Get and set world position for a grid node
    public int GridPosX { get; set; }   // Get and set grid X position for a node
    public int GridPosY { get; set; }   // Get and set grid Y position for a node
    public int GScore { get; set; }     // Get and set the score of the path from the start node to next node
    public int HScore { get; set; }     // Get and set the score of the path from the next node to end node
    public int FScore { get { return GScore + HScore; } }   // Get the score of the path (g + h)
    public GridNode Parent { get; set; }   // Get and set the parent of grid node
    public int HeapIndex { get; set; }      // Get and set the heap data index (HeapMin)

    /// <summary>
    /// Constructor for grid node object.
    /// </summary>
    /// <param name="isObstacle">Is grid node obstacle or node is clear.</param>
    /// <param name="worldPosition">World position of grid node.</param>
    /// <param name="gridPosX">Grid X position of node.</param>
    /// <param name="gridPosY">Grid Y position of node.</param>
    public GridNode(bool isObstacle, Vector3 worldPosition, int gridPosX, int gridPosY)
    {
        IsObstacle = isObstacle;
        WorldPosition = worldPosition;
        GridPosX = gridPosX;
        GridPosY = gridPosY;
    }

    /// <summary>
    /// Compare two grid nodes to see which one has lower score.
    /// </summary>
    /// <param name="nodeToCompare">Grid node to compare with.</param>
    /// <returns>Returns 1 if smaller score, or -1 if bigger score.</returns>
    public int CompareTo(GridNode nodeToCompare)
    {
        int result = FScore.CompareTo(nodeToCompare.FScore);
        if(result == 0)
        {
            result = HScore.CompareTo(nodeToCompare.HScore);
        }
        return -result;
    }
}
