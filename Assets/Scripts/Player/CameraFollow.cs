using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        Vector3 smoothPosition = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = new Vector3(
            smoothPosition.x,
            smoothPosition.y,
            transform.position.z
        );
    }
}