using UnityEngine;

public class MouseClickPosition : MonoBehaviour
{
    [Tooltip("Layer mask registered by mouse click")]
    public LayerMask clickLayerMask;
    [Tooltip("Game object representing start")]
    public Transform startPoint;
    [Tooltip("Game object representing end")]
    public Transform endPoint;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveObjectToMousePos(startPoint);
        }

        if (Input.GetMouseButtonDown(1))
        {
            MoveObjectToMousePos(endPoint);
        }
    }

    private void MoveObjectToMousePos(Transform gameObj)
    {
        // Raycast from mouse world position towards ground
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Mathf.Abs(Camera.main.transform.position.z) + 1;
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Camera.main.transform.forward, distance, clickLayerMask);

        // Move object only if we hit ground layer
        if (hit.collider != null)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                gameObj.position = hit.point;
            }
        }
    }
}
