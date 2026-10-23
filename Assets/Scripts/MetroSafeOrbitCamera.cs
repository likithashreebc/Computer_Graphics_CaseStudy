using UnityEngine;

public class MetroSafeOrbitCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 30f;
    public float minDistance = 14f;
    public float maxDistance = 42f;
    public float sensitivity = 3f;
    float yaw = 0f;
    float pitch = 36f;

    void LateUpdate()
    {
        if (target == null) return;
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;
            pitch = Mathf.Clamp(pitch, 18f, 72f);
        }
        distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y * 1.2f, minDistance, maxDistance);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = target.position + rotation * new Vector3(0, 0, -distance);
        transform.rotation = rotation;
        transform.LookAt(target.position + Vector3.up * .7f);
    }
}
