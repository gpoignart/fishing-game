using System;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    // Reference to the managers
    public UIManager uiManager;
    public GameManager gameManager;

    // Timer settings
    public float startTime = 60f;
    public float bonusTimePerFish = 5f;
    public float maxExtraTime = 15f; // Allows exceeding startTime slightly

    // Runtime state
    public float TimeRemaining { get; private set; }
    public int FishCount { get; private set; }

    public void Reset()
    {
        TimeRemaining = startTime;
        FishCount = 0;

        if (uiManager != null)
        {
            uiManager.UpdateTimerDisplay(TimeRemaining);
            uiManager.UpdateFishCounterDisplay(FishCount);
            uiManager.SetTimerColor(new Color(1f, 0.694f, 0f));
        }
        else
        {
            Debug.LogWarning("UIManager reference missing in GameSession!");
        }
    }

    public void UpdateTimer(float deltaTime)
    {
        TimeRemaining -= deltaTime;

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            uiManager?.SetTimerColor(Color.red);
            gameManager.OnTimeout();
        }

        uiManager?.UpdateTimerDisplay(TimeRemaining);
    }

    public void AddCatch()
    {
        FishCount++;
        TimeRemaining = Mathf.Min(TimeRemaining + bonusTimePerFish, startTime + maxExtraTime);

        uiManager?.UpdateFishCounterDisplay(FishCount);
        uiManager?.UpdateTimerDisplay(TimeRemaining);
        uiManager?.PlayCatchAnimation();
    }
}
