using UnityEngine;
using UnityEngine.Events;

public class OculusCursorCanvasDetector : MonoBehaviour
{
    [Tooltip("Time in seconds to debounce entry/exit detection")]
    [SerializeField] private float debounceTime = 0.1f;

    private float lastStateChangeTime;
    private bool wasCursorOnCanvas = false;

    public UnityEvent OnCursorEnterCanvas;
    public UnityEvent OnCursorExitCanvas;

    void Update()
    {
        float currentTime = Time.time;
        bool isCursorOnCanvas = false;

        // Find all GameObjects named "OculusCursor"
        GameObject[] cursors = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in cursors)
        {
            if (obj.name != "OculusCursor")
                continue;

            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null && renderer.enabled)
            {
                isCursorOnCanvas = true;
                break;
            }
        }

        // Trigger entry
        if (isCursorOnCanvas && !wasCursorOnCanvas && TimePassed(currentTime))
        {
            Debug.Log("▶ Oculus cursor entered canvas.");
            OnCursorEnterCanvas?.Invoke();
            lastStateChangeTime = currentTime;
        }

        // Trigger exit
        if (!isCursorOnCanvas && wasCursorOnCanvas && TimePassed(currentTime))
        {
            Debug.Log("◀ Oculus cursor exited canvas.");
            OnCursorExitCanvas?.Invoke();
            lastStateChangeTime = currentTime;
        }

        wasCursorOnCanvas = isCursorOnCanvas;
    }

    private bool TimePassed(float currentTime)
    {
        return (currentTime - lastStateChangeTime) > debounceTime;
    }
}
