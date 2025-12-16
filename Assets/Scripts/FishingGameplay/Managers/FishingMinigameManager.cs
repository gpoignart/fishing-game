using UnityEngine;
using System.Linq;

public class FishingMinigameManager : MonoBehaviour
{
    // Allows to call FishingMinigameManager.Instance anywhere (singleton)
    public static FishingMinigameManager Instance { get; private set; }

    // Parameters
    private float needleMoveSpeed = 200f;

    // Counters of times
    private float timeInsideZone = 0f;
    private float timeOutsideZone = 0f;

    // Internal references
    private FishCatchingDifficulty difficulty;
    private Vector2 needleTargetPosition;
    private Vector2 safeZoneTargetPosition;
    private Vector2 needleLeftBoundaryPosition;
    private Vector2 needleRightBoundaryPosition;
    private Vector2 safeZoneLeftBoundaryPosition;
    private Vector2 safeZoneRightBoundaryPosition;

    // Make this class a singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartMiniGame(Fish fish)
    {
        // Reset counters
        timeInsideZone = 0f;
        timeOutsideZone = 0f;

        // Initialize difficulty depending the fish and the timeOfDay
        difficulty = fish.fishSO.catchingDifficulties.FirstOrDefault(t => t.time == GameManager.Instance.CurrentTimeOfDay);

        // Set up needle boundaries positions
        float needleHalfWidth = FishingMinigameUIManager.Instance.GetNeedleWidth() / 2f;
        needleLeftBoundaryPosition = new Vector2(FishingMinigameUIManager.Instance.GetLeftBoundaryPosition().x + needleHalfWidth, FishingMinigameUIManager.Instance.GetLeftBoundaryPosition().y);
        needleRightBoundaryPosition = new Vector2(FishingMinigameUIManager.Instance.GetRightBoundaryPosition().x - needleHalfWidth, FishingMinigameUIManager.Instance.GetRightBoundaryPosition().y);

        // Start and target position for needle
        FishingMinigameUIManager.Instance.SetNeedlePosition(needleLeftBoundaryPosition);

        needleTargetPosition = needleLeftBoundaryPosition;

        // Adapt the safeZone width to the difficulty
        FishingMinigameUIManager.Instance.SetSafeZoneWidth(difficulty.safeZoneWidth);

        // Set up safeZone boundaries positions
        float safeZoneHalfWidth = difficulty.safeZoneWidth / 2f;
        safeZoneLeftBoundaryPosition = new Vector2(FishingMinigameUIManager.Instance.GetLeftBoundaryPosition().x + safeZoneHalfWidth, FishingMinigameUIManager.Instance.GetLeftBoundaryPosition().y);
        safeZoneRightBoundaryPosition = new Vector2(FishingMinigameUIManager.Instance.GetRightBoundaryPosition().x - safeZoneHalfWidth, FishingMinigameUIManager.Instance.GetRightBoundaryPosition().y);

        // Safe zone picks a random target
        safeZoneTargetPosition = GetRandomSafeZonePosition();
    }

    public void UpdateMiniGame()
    {
        // Safe zone movements
        FishingMinigameUIManager.Instance.MoveSafeZone(safeZoneTargetPosition, difficulty.safeZoneMoveSpeed * Time.deltaTime);

        // When the safeZone reach its target, it changes randomly of target
        if (Vector2.Distance(FishingMinigameUIManager.Instance.GetSafeZonePosition(), safeZoneTargetPosition) < 0.1f)
        {
            safeZoneTargetPosition = GetRandomSafeZonePosition();
        }

        // Needle movements
        needleTargetPosition = Input.GetKey(KeyCode.Space)
            ? needleRightBoundaryPosition       // Space → pulled right
            : needleLeftBoundaryPosition;       // Default → pulled left

        FishingMinigameUIManager.Instance.MoveNeedle(needleTargetPosition, needleMoveSpeed * Time.deltaTime);

        // Check if needle is inside safe zone
        if (FishingMinigameUIManager.Instance.IsNeedleInsideSafeZone())
        {
            timeInsideZone += Time.deltaTime;
            timeOutsideZone = 0f;

            if (timeInsideZone >= difficulty.requiredTimeInsideZone)
            {
                FishingGameManager.Instance.FishingMinigameSuccess();
            }
        }
        else
        {
            timeInsideZone = 0f;
            timeOutsideZone += Time.deltaTime;

            if (timeOutsideZone >= difficulty.allowedTimeOutsideZone)
            {
                FishingGameManager.Instance.FishingMinigameFail();
            }
        }
    }
    
    private Vector2 GetRandomSafeZonePosition()
    {
        float randomX = Random.Range(safeZoneLeftBoundaryPosition.x, safeZoneRightBoundaryPosition.x);
        return new Vector2(randomX, FishingMinigameUIManager.Instance.GetSafeZonePosition().y);
    }
}


