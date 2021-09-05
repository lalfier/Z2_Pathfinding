using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Tooltip("Starting camera aspect, in this case 16/9 (width/height)")]
    public float defaultAspect = 1.777778f;
    [Tooltip("Starting camera size, in this case 52")]
    public float defaultSize = 52f;

    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        if (camera.aspect <= defaultAspect)
        {
            // On application start set bigger camera size for smaller aspect. Whole scene must be visible.
            camera.orthographicSize = 1 / camera.aspect * defaultAspect * defaultSize;
        }
    }
}
