using UnityEngine;

public class Difficulty
{
    public float safeZoneMoveSpeed = 100f;
    public float requiredTimeInsideZone = 3f;
    public float allowedTimeOutsideZone = 3f;
    public float safeZoneWidth = 100f;
}

public class FishingMinigame : MonoBehaviour
{
    public FishingGameManager gameManager;

    // Boundaries for movement
    public Transform leftBoundary;
    public Transform rightBoundary;

    // UI
    public RectTransform safeZoneUI;

    // Difficulty
    private Difficulty difficulty = new Difficulty();

    // Speed of needle
    public float needleMoveSpeed = 200f;

    // Internal state
    private float timeInsideZone = 0f;
    private float timeOutsideZone = 0f;
    private bool isGameLocked = false;

    // Internal references
    private RectTransform needleTransform;
    private Vector2 needleTargetPosition;
    private Vector2 safeZoneTargetPosition;
    private Vector2 needleLeftBoundaryPosition;
    private Vector2 needleRightBoundaryPosition;
    private Vector2 safeZoneLeftBoundaryPosition;
    private Vector2 safeZoneRightBoundaryPosition;

    void Awake()
    {
        needleTransform = GetComponent<RectTransform>();
    }

    public void StartMiniGame()
    {
        isGameLocked = false;
        timeInsideZone = 0f;
        timeOutsideZone = 0f;

        // Set up needle boundaries positions
        float needleHalfWidth = needleTransform.rect.width / 2f;
        needleLeftBoundaryPosition = new Vector2(leftBoundary.position.x + needleHalfWidth, leftBoundary.position.y);
        needleRightBoundaryPosition = new Vector2(rightBoundary.position.x - needleHalfWidth, rightBoundary.position.y);

        // Start and target position for needle
        needleTransform.position = needleLeftBoundaryPosition;
        needleTargetPosition = needleLeftBoundaryPosition;

        // Adapt the safeZone width to the difficulty
        safeZoneUI.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, difficulty.safeZoneWidth);

        // Set up safeZone boundaries positions
        float safeZoneHalfWidth = difficulty.safeZoneWidth / 2f;
        safeZoneLeftBoundaryPosition = new Vector2(leftBoundary.position.x + safeZoneHalfWidth, leftBoundary.position.y);
        safeZoneRightBoundaryPosition = new Vector2(rightBoundary.position.x - safeZoneHalfWidth, rightBoundary.position.y);

        // Safe zone picks a random target
        safeZoneTargetPosition = GetRandomSafeZonePosition();
    }

    public void UpdateMiniGame()
    {
        if (isGameLocked)
            return;

        // --- SAFE ZONE MOVEMENT ---
        safeZoneUI.position = Vector2.MoveTowards(
            safeZoneUI.position,
            safeZoneTargetPosition,
            difficulty.safeZoneMoveSpeed * Time.deltaTime
        );

        // When the safeZone reach its target, it changes randomly of target
        if (Vector2.Distance(safeZoneUI.position, safeZoneTargetPosition) < 0.1f)
        {
            safeZoneTargetPosition = GetRandomSafeZonePosition();
        }

        // --- NEEDLE MOVEMENT ---
        needleTargetPosition = Input.GetKey(KeyCode.Space)
            ? needleRightBoundaryPosition       // Space → pulled right
            : needleLeftBoundaryPosition;        // Default → pulled left

        needleTransform.position = Vector2.MoveTowards(
            needleTransform.position,
            needleTargetPosition,
            needleMoveSpeed * Time.deltaTime
        );

        // --- CHECK IF NEEDLE IS IN SAFE ZONE ---
        bool isInsideSafeZone = RectTransformUtility.RectangleContainsScreenPoint(
            safeZoneUI,
            needleTransform.position,
            null
        );

        if (isInsideSafeZone)
        {
            timeInsideZone += Time.deltaTime;
            timeOutsideZone = 0f;

            if (timeInsideZone >= difficulty.requiredTimeInsideZone)
            {
                isGameLocked = true;
                gameManager.OnDragSuccess();
            }
        }
        else
        {
            timeInsideZone = 0f;
            timeOutsideZone += Time.deltaTime;

            if (timeOutsideZone >= difficulty.allowedTimeOutsideZone)
            {
                isGameLocked = true;
                gameManager.OnDragFail();
            }
        }
    }

    private Vector2 GetRandomSafeZonePosition()
    {
        float randomX = Random.Range(safeZoneLeftBoundaryPosition.x, safeZoneRightBoundaryPosition.x);
        return new Vector2(randomX, safeZoneUI.position.y);
    }
}


