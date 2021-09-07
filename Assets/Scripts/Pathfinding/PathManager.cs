using UnityEngine;

[RequireComponent(typeof(PathfindingLogic))]
public class PathManager : MonoBehaviour
{
    private static PathManager instance;    // Reference to itself (Singleton)
    private PathfindingLogic pathfinding;   // Pathfinding logic reference

    private void Awake()
    {
        // Check if PathManager already exists (Singleton)
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        pathfinding = GetComponent<PathfindingLogic>();
    }

    /// <summary>
    /// Request path calculation from start to end position using A* pathfinding algorithm.
    /// </summary>
    /// <param name="startPath">Start world position.</param>
    /// <param name="endPath">End world position.</param>
    /// <param name="returnAllWaypoints">Return all waypoints from path calculations or ones with direction change.</param>
    /// <returns>Returns array of path waypoints (world position).</returns>
    public static Vector3[] RequestPath(Vector3 startPath, Vector3 endPath, bool returnAllWaypoints)
    {
        return instance.pathfinding.CalculatePath(startPath, endPath, returnAllWaypoints);
    }
}
