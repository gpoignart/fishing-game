using UnityEngine;
using UnityEngine.UI;

public class FishingMinigameUIManager : MonoBehaviour
{
    // Singleton instance
    public static FishingMinigameUIManager Instance { get; private set; }

    // References to boundary and UI elements
    [SerializeField] private RectTransform leftBoundaryUI;
    [SerializeField] private RectTransform rightBoundaryUI;
    [SerializeField] private RectTransform safeZoneUI;
    [SerializeField] private RectTransform needleUI;
    [SerializeField] private Image fillRight; // Progress bar right
    [SerializeField] private Image fillLeft;  // Progress bar left
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Get positions of UI elements
    public Vector2 GetLeftBoundaryPosition() { return leftBoundaryUI.anchoredPosition; }
    public Vector2 GetRightBoundaryPosition() { return rightBoundaryUI.anchoredPosition; }
    public Vector2 GetSafeZonePosition() { return safeZoneUI.anchoredPosition; }
    public Vector2 GetNeedlePosition() { return needleUI.anchoredPosition; }
    public float GetNeedleWidth() { return needleUI.rect.width; }

    // Set needle position
    public void SetNeedlePosition(Vector2 position) { needleUI.anchoredPosition = position; }

    // Adjust safe zone width
    public void SetSafeZoneWidth(float size)
    {
        safeZoneUI.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
    }

    // Move the safe zone towards a target position
    public void MoveSafeZone(Vector2 target, float maxDistanceDelta)
    {
        safeZoneUI.anchoredPosition = Vector2.MoveTowards(
            safeZoneUI.anchoredPosition, target, maxDistanceDelta
        );
    }

    // Move the needle towards a target position
    public void MoveNeedle(Vector2 target, float maxDistanceDelta)
    {
        needleUI.anchoredPosition = Vector2.MoveTowards(
            needleUI.anchoredPosition, target, maxDistanceDelta
        );
    }

    // Check if the needle is inside the safe zone
    public bool IsNeedleInsideSafeZone()
    {
        float halfWidth = safeZoneUI.rect.width / 2f;
        float needleX = needleUI.anchoredPosition.x;
        float safeZoneX = safeZoneUI.anchoredPosition.x;

        return needleX >= safeZoneX - halfWidth && needleX <= safeZoneX + halfWidth;
    }

    // Initialize Progress Bar
    public void InitializeProgressBar()
    {
        if (fillRight)
        {
            fillRight.fillAmount = 0f;
        }

        if (fillLeft)
        {
            fillLeft.fillAmount = 0f;
        }
    }

    // Update progress bar
    public void UpdateProgressBar(float timeInsideZone, float requiredTimeInsideZone, float timeOutsideZone, float allowedTimeOutsideZone)
    {
        float ratioInside = Mathf.Clamp01(timeInsideZone / requiredTimeInsideZone);
        float ratioOutside = Mathf.Clamp01(timeOutsideZone / allowedTimeOutsideZone);

        if (fillRight)
        {
            fillRight.fillAmount = ratioInside;
        }

        if (fillLeft)
        {
            fillLeft.fillAmount = ratioOutside;
        }
    }
}
