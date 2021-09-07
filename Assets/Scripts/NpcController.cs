using UnityEngine;

public class NpcController : MonoBehaviour
{
    [Tooltip("Return all waypoints from path calculations or ones with direction change")]
    public bool returnAllWaypoints = true;
    [Tooltip("Game object representing target/end point")]
    public Transform target;

    private Vector3[] waypoints;    // Array of path waypoints (world position)

    private void Start()
    {
        UpdatePathWaypoints();
    }

    /// <summary>
    /// Update path waypoints from npc to target position using A* pathfinding algorithm.
    /// </summary>
    public void UpdatePathWaypoints()
    {
        waypoints = PathManager.RequestPath(transform.position, target.position, returnAllWaypoints);
    }

    private void OnDrawGizmos()
    {
        if(waypoints != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                Gizmos.color = Color.magenta;
                // Draw waypoints
                Gizmos.DrawCube(waypoints[i], Vector2.one);
                // Draw lines between waypoints
                if (!returnAllWaypoints)
                {
                    if (i == 0)
                    {
                        Gizmos.DrawLine(transform.position, waypoints[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(waypoints[i - 1], waypoints[i]);
                    }
                }               
            }
        }
    }
}
