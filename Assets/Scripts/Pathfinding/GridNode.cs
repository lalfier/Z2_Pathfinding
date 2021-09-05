using UnityEngine;

public class GridNode
{
    public bool IsObstacle { get;  set; }   // Get and set is grid node an obstacle
    public Vector3 WorldPosition { get; set; }  // Get and set world position for a grid node

    /// <summary>
    /// Constructor for grid node object.
    /// </summary>
    /// <param name="isObstacle">Is grid node obstacle or node is clear.</param>
    /// <param name="worldPosition">World position of grid node.</param>
    public GridNode(bool isObstacle, Vector3 worldPosition)
    {
        IsObstacle = isObstacle;
        WorldPosition = worldPosition;
    }
}
