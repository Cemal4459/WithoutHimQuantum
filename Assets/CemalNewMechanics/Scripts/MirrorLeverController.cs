using UnityEngine;

public class MirrorLeverController : MonoBehaviour
{
    public Transform leftHandle;
    public Transform rightHandle;

    public float minY = -0.7f;
    public float maxY = 1.5f;
    public float maxMirrorAngle = 60f;
    public float rotationSmooth = 10f;

    public float clickDistance = 1.2f;

    private Transform selectedHandle;
    private Camera cam;

void Start()
{
    cam = Camera.main;

    Vector3 leftPos = leftHandle.position;
    leftPos.y = maxY;
    leftHandle.position = leftPos;

    Vector3 rightPos = rightHandle.position;
    rightPos.y = minY;
    rightHandle.position = rightPos;
}

    void Update()
    {
        if (cam == null) return;

        Vector3 mouse3D = Input.mousePosition;
        mouse3D.z = Mathf.Abs(cam.transform.position.z);
        Vector2 mouseWorld = cam.ScreenToWorldPoint(mouse3D);

        if (Input.GetMouseButtonDown(0))
        {
            float leftDistance = Vector2.Distance(mouseWorld, leftHandle.position);
            float rightDistance = Vector2.Distance(mouseWorld, rightHandle.position);

            Debug.Log("Left Distance: " + leftDistance);
            Debug.Log("Right Distance: " + rightDistance);

            if (leftDistance <= clickDistance)
            {
                selectedHandle = leftHandle;
                Debug.Log("LEFT HANDLE SELECTED");
            }
            else if (rightDistance <= clickDistance)
            {
                selectedHandle = rightHandle;
                Debug.Log("RIGHT HANDLE SELECTED");
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedHandle = null;
        }

        if (selectedHandle != null)
        {
            Vector3 pos = selectedHandle.position;
            pos.y = Mathf.Clamp(mouseWorld.y, minY, maxY);
            selectedHandle.position = pos;
        }

        RotateMirror();
    }

    void RotateMirror()
    {
        float difference = rightHandle.position.y - leftHandle.position.y;
        float range = maxY - minY;

        float normalized = Mathf.Clamp(difference / range, -1f, 1f);
        float targetAngle = normalized * maxMirrorAngle;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, targetAngle),
            rotationSmooth * Time.deltaTime
        );
    }
}