using UnityEngine;

public class DragMinigame : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public float moveSpeed = 100f;
    public float nudgeDistance = 20f;  
  
    public float lockThreshold = 3f;   // số giây cần ở trong safe zone
    public float outLoseTime = 1.5f;
    private float inSafeTimer = 0f;
    private float outTimer = 0f;
    private bool locked = false;

    private float direction = 1f;      
    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    public System.Action onDragSuccess;
    public System.Action onDragFail;
    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition   = pointB.position;
    }

    void Update()
    {
        if (locked) return;
        pointerTransform.position = Vector3.MoveTowards(
            pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var backTarget = (direction > 0f) ? pointA.position : pointB.position;
            pointerTransform.position = Vector3.MoveTowards(
                pointerTransform.position, backTarget, nudgeDistance);

            CheckSuccess(); // vẫn check QTE nếu bạn muốn
        }

        // 4) Đếm thời gian ở trong safe zone rồi khóa sau lockThreshold giây
        bool inSafe = RectTransformUtility.RectangleContainsScreenPoint(
            safeZone, pointerTransform.position, null); // nếu Canvas có Camera, truyền camera thay null

        if (inSafe)
        {
            inSafeTimer += Time.deltaTime;
            outTimer = 0f;

            if (inSafeTimer >= lockThreshold)
            {
                locked = true;
                Debug.Log("Locked: stayed in safe zone for " + lockThreshold + "s");
                onDragSuccess?.Invoke();   // báo GameManager: thắng
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
                onDragFail?.Invoke();      // báo GameManager: thua
            }
        }
    }

    void CheckSuccess()
    {
        bool inSafe = RectTransformUtility.RectangleContainsScreenPoint(
            safeZone, pointerTransform.position, null);

        if (inSafe) Debug.Log("Success!");
        else        Debug.Log("Fail!");
    }

    // Gọi hàm này khi muốn chơi lại lượt mới
    public void ResetPointer()
    {
        locked = false;
        inSafeTimer = 0f;
        direction = 1f;
        targetPosition = pointB.position;
    }
}
