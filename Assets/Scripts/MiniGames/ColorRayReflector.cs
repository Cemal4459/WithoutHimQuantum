using UnityEngine;

public class ColorRayReflector : MonoBehaviour
{
    [Header("References")]
    public Transform lightSource;
    public Transform safeZone;

    [Header("Layers")]
    public LayerMask glassLayer;
    public LayerMask groundLayer;

    [Header("Ray Settings")]
    public float rayLength = 20f;
    public float colorRayLength = 20f;

    [Header("Line Visual")]
    public LineRenderer incomingLine;
    public LineRenderer colorLine;

    [Header("Glass Direction")]
    public bool useNegativeDirection = false;

    [Header("Safe Zone")]
    public float safeZoneWidth = 3.5f;
    public float safeZoneHeight = 0.28f;

    [Header("Beam Width")]
    public float incomingWidth = 0.05f;
    public float beamStartWidth = 0.04f;
    public float beamEndWidth = 1.2f;

    private SpriteRenderer safeZoneSprite;
    private Collider2D safeZoneCollider;

    void Start()
    {
        safeZoneSprite = safeZone.GetComponent<SpriteRenderer>();
        safeZoneCollider = safeZone.GetComponent<Collider2D>();

        SetupIncomingLine();
        SetupColorBeam();

        HideEverything();
    }

    void Update()
    {
        ShootRay();
    }

    void ShootRay()
    {
        HideEverything();

        Vector2 start = lightSource.position;
        Vector2 incomingDirection = Vector2.down;

        RaycastHit2D glassHit = Physics2D.Raycast(
            start,
            incomingDirection,
            rayLength,
            glassLayer
        );

        Vector2 ceilingStart = start + Vector2.up * 8f;
SetLine(incomingLine, ceilingStart, glassHit.point);
        SetLine(colorLine, Vector2.zero, Vector2.zero);

        if (!glassHit.collider)
            return;

        SetLine(incomingLine, start, glassHit.point);

        Vector2 colorDirection = -glassHit.collider.transform.up;

        if (useNegativeDirection)
            colorDirection = -colorDirection;

        RaycastHit2D groundHit = Physics2D.Raycast(
            glassHit.point + colorDirection * 0.05f,
            colorDirection,
            colorRayLength,
            groundLayer
        );

        if (!groundHit.collider)
        {
            Vector2 endPoint = glassHit.point + colorDirection * colorRayLength;
            SetLine(colorLine, glassHit.point, endPoint);
            return;
        }

        Vector2 extendedEnd = groundHit.point + colorDirection * 1.8f;

 SetLine(colorLine, glassHit.point, extendedEnd);

float beamDistance = Vector2.Distance(glassHit.point, groundHit.point);
ShowSafeZone(groundHit.point, beamDistance);
    }

void ShowSafeZone(Vector2 pos, float beamDistance)
{
    float width = Mathf.Clamp(beamDistance * 0.45f, 1.8f, 3.6f);
    float height = 0.32f;

    safeZone.position = new Vector3(
        pos.x,
        pos.y + 0.05f,
        safeZone.position.z
    );

    safeZone.localScale = new Vector3(width, height, 1f);

    if (safeZoneSprite != null)
    {
        safeZoneSprite.enabled = true;
        safeZoneSprite.color = new Color(1f, 1f, 1f, 0f);
    }

    if (safeZoneCollider != null)
        safeZoneCollider.enabled = true;
}

    void HideEverything()
    {
        if (safeZoneSprite != null)
            safeZoneSprite.enabled = false;

        if (safeZoneCollider != null)
            safeZoneCollider.enabled = false;
    }

    void SetupIncomingLine()
    {
        if (incomingLine == null) return;

        incomingLine.positionCount = 2;
  incomingLine.startWidth = 0.12f;
incomingLine.endWidth = 0.12f;
        incomingLine.useWorldSpace = true;
    }

    void SetupColorBeam()
    {
        if (colorLine == null) return;

        colorLine.positionCount = 2;
        colorLine.startWidth = beamStartWidth;
        colorLine.endWidth = beamEndWidth;
        colorLine.useWorldSpace = true;
    }

    void SetLine(LineRenderer line, Vector2 start, Vector2 end)
    {
        if (line == null) return;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}