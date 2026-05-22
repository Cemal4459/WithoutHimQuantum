using UnityEngine;

public class MirrorLeverController : MonoBehaviour
{
    public Transform leftHandle;
    public Transform rightHandle;

    public float minY = -1.75f;
    public float maxY = 0.8f;

    public float maxMirrorAngle = 62.5f;
    public float rotationSmooth = 10f;

    public float clickDistance = 1.2f;

    private Transform selectedHandle;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        // Oyun başında ışık EN SAĞDA başlasın
        // Sol kol aşağıda, sağ kol yukarıda
        Vector3 leftPos = leftHandle.position;
        leftPos.y = minY;
        leftHandle.position = leftPos;

        Vector3 rightPos = rightHandle.position;
        rightPos.y = maxY;
        rightHandle.position = rightPos;

        RotateMirrorInstant();
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

            if (leftDistance <= clickDistance)
            {
                selectedHandle = leftHandle;
            }
            else if (rightDistance <= clickDistance)
            {
                selectedHandle = rightHandle;
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

        RotateMirrorSmooth();
    }

    void RotateMirrorSmooth()
    {
        float difference = rightHandle.position.y - leftHandle.position.y;
        float range = maxY - minY;

        float normalized = Mathf.Clamp(difference / range, -1f, 1f);
        float targetAngle = normalized * maxMirrorAngle;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0f, 0f, targetAngle),
            rotationSmooth * Time.deltaTime
        );
    }

    void RotateMirrorInstant()
    {
        float difference = rightHandle.position.y - leftHandle.position.y;
        float range = maxY - minY;

        float normalized = Mathf.Clamp(difference / range, -1f, 1f);
        float targetAngle = normalized * maxMirrorAngle;

        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
    }
    public void ResetLeverToStart()
{
    Vector3 leftPos = leftHandle.position;
    leftPos.y = minY;
    leftHandle.position = leftPos;

    Vector3 rightPos = rightHandle.position;
    rightPos.y = maxY;
    rightHandle.position = rightPos;

    RotateMirrorInstant();
}
}