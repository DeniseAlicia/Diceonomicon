using UnityEngine;
using System.Collections;

public class CameraOrbitController : MonoBehaviour
{
    [Header("Orbit Target")]
    public Transform centerTransform;
    public Transform wpTransform;
    public float distance = 15f;    // constant radius around center
    public float rotateSpeed = 120f;
    public float focusSpeed = 3f;
    public float edgeScrollSpeed = 10f;
    public float topScreenPercent = 0.1f;
    public float bottomScreenPercent = 0.1f;

    float yaw = 0f;
    bool isFocusing = false;
    float currentHeight;

    void Start()
    {
        if (centerTransform != null)
        {
            Vector3 dir = transform.position - centerTransform.position;
            dir.y = 0;
            yaw = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

            currentHeight = transform.position.y;
        }
    }

    void Update()
    {
        HandleMouseOrbit();
        HandleEdgeScroll();
    }

    void LateUpdate()
    {
        if (wpTransform != null)
        {
            Vector3 offset = Quaternion.Euler(0f, yaw, 0f) * Vector3.right * distance/1.5f;
            transform.position = new Vector3(offset.x, currentHeight, offset.z) + wpTransform.position;
            transform.LookAt(wpTransform.position);
        }
        else if (centerTransform != null)
        {
            Vector3 offset = Quaternion.Euler(0f, yaw, 0f) * Vector3.right * distance;
            transform.position = new Vector3(offset.x, currentHeight, offset.z) + centerTransform.position;
            transform.LookAt(centerTransform.position);
        }
    }

    void HandleMouseOrbit()
    {
        if (Input.GetMouseButton(1) && !isFocusing) // right click drag
        {
            float dx = Input.GetAxis("Mouse X");
            yaw += dx * rotateSpeed * Time.deltaTime;
        }
    }

    void HandleEdgeScroll()
    {
        if (centerTransform == null) return;

        float mouseY = Input.mousePosition.y;
        float topEdge = Screen.height * (1f - topScreenPercent);
        float bottomEdge = Screen.height * bottomScreenPercent;

        if (mouseY > topEdge)
        {
            currentHeight += edgeScrollSpeed * Time.deltaTime;
        }
        if (mouseY < bottomEdge)
        {
            currentHeight -= edgeScrollSpeed * Time.deltaTime;
        }

        // Clamp height to prevent going too low or high
        currentHeight = Mathf.Clamp(currentHeight, -50f, 50f);
    }

    public void FocusOnNode(Transform node)
    {
        if (centerTransform == null || node == null) return;

        Waypoint waypoint = node.GetComponent<Waypoint>();
        // Get branch direction
        Vector3 branchDir = waypoint.GetBranchDirection();

        // Calculate the yaw from branch direction instead of node position
        float targetYaw = Mathf.Atan2(branchDir.z, branchDir.x) * Mathf.Rad2Deg;

        // Use node's Y position to set height
        float targetY = node.transform.position.y;

        StartCoroutine(FocusRoutine(targetYaw, targetY, 0.6f));
    }

    IEnumerator FocusRoutine(float targetYaw, float targetY, float duration)
    {
        isFocusing = true;
        float startYaw = yaw;
        float delta = Mathf.DeltaAngle(startYaw, targetYaw);
        float finalYaw = startYaw + delta;

        float startY = currentHeight;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            yaw = Mathf.Lerp(startYaw, finalYaw, t);
            currentHeight = Mathf.Lerp(startY, targetY, t);

            yield return null;
        }

        yaw = finalYaw;
        isFocusing = false;
    }
}
