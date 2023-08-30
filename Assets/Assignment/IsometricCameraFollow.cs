using UnityEngine;
public class IsometricCameraFollow : MonoBehaviour
{
    public Transform target;        // Reference to the character's Transform
    public Vector3 offset;          // Offset between camera and character
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
