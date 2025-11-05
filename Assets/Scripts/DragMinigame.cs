using UnityEngine;

public class DragMinigame : MonoBehaviour
{
    public GameManager gameManager;

    public Transform pointA;
    public Transform pointB;
    public Transform startPoint;
    public RectTransform safeZone;
    public float moveSpeed = 200f;
    public float moveSafeZoneSpeed = 100f;
    public float nudgeDistance = 80f;

    public float lockThreshold = 3f;
    public float outLoseTime = 3f;
    private float inSafeTimer = 0f;
    private float outTimer = 0f;
    private bool locked = false;

    private float direction = 1f;
    private float safeZonedirection = -1f;
    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private Vector3 targetSafeZonePosition;

    void Awake()
    {
        pointerTransform = GetComponent<RectTransform>();
    }

    public void StartMiniGame()
    {
        locked = false;
        inSafeTimer = 0f;
        outTimer = 0f;
        direction = 1f;
        safeZonedirection = -1f;

        pointerTransform.position = startPoint.position;
        targetPosition = pointB.position;
        targetSafeZonePosition = pointA.position;
    }

    public void UpdateMiniGame()
    {
        if (locked)
            return;

        // Make the safe zone move
        safeZone.position = Vector3.MoveTowards(
            safeZone.position,
            targetSafeZonePosition,
            moveSafeZoneSpeed * Time.deltaTime
        );

        pointerTransform.position = Vector3.MoveTowards(
            pointerTransform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
            direction = 1f;
        }
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
            direction = -1f;
        }

        if (Vector3.Distance(safeZone.position, pointA.position) < 0.1f)
        {
            targetSafeZonePosition = pointB.position;
            safeZonedirection = 1f;
        }
        else if (Vector3.Distance(safeZone.position, pointB.position) < 0.1f)
        {
            targetSafeZonePosition = pointA.position;
            safeZonedirection = -1f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var backTarget = (direction > 0f) ? pointA.position : pointB.position;
            pointerTransform.position = Vector3.MoveTowards(
                pointerTransform.position,
                backTarget,
                nudgeDistance
            );
        }

        // Time in safe zone
        bool inSafe = RectTransformUtility.RectangleContainsScreenPoint(
            safeZone,
            pointerTransform.position,
            null
        );

        if (inSafe)
        {
            inSafeTimer += Time.deltaTime;
            outTimer = 0f;

            if (inSafeTimer >= lockThreshold)
            {
                locked = true;
                Debug.Log("Locked: stayed in safe zone for " + lockThreshold + "s");
                gameManager.OnDragSuccess();
            }
        }
        else
        {
            inSafeTimer = 0f;
            outTimer += Time.deltaTime;

            if (outTimer >= outLoseTime)
            {
                locked = true;
                Debug.Log("Drag Lose (out of safe zone too long)");
                gameManager.OnDragFail();
            }
        }
    }
}
