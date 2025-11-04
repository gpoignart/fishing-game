using UnityEngine;
using TMPro;
using System;

public class GameSession : MonoBehaviour
{
    public float startTime = 60f;
    public float bonusTimePerFish = 5f;
    public float maxTimeCapExtra = 15f;   // allow exceeding startTime by a small cap

    // UI hook so we can push labels from the session (used in Reset/UpdateTimer/AddCatch)
    public UIManager ui;                  // assign in Inspector to avoid NullReference

    private TextMeshProUGUI timerText;    // optional: assign if you want to change timer text color

    // Runtime
    public float TimeRemaining { get; private set; }
    public int   FishCount     { get; private set; }

    public Action OnTimeout;   // GameManager can subscribe to this to handle timeout

    bool start = false;        // acts like a "running" flag: true = ticking
    bool firedTimeout = false; // ensure OnTimeout fires only once per round

    // --- API expected by GameManager ---
    public void Reset()
    {
        TimeRemaining = startTime;
        FishCount = 0;
        start = false;                     // FIX: previously 'running = false;' (undefined)
        firedTimeout = false;

        // Push initial HUD values (requires 'ui' reference)
        ui.UpdateTimerDisplay(TimeRemaining);
        ui.UpdateFishCounterDisplay(FishCount);
    }

    // Called every frame by GameManager: countdown behavior
    public void UpdateTimer(float dt)
    {
        if (!start) start = true; // begin ticking on first call
        if (!start) return;

        if (TimeRemaining > 0f)
        {
            TimeRemaining -= dt;
            if (TimeRemaining <= 0f)
            {
                TimeRemaining = 0f;
                start = false;
                SetTimerColor(Color.red);
                if (!firedTimeout) { firedTimeout = true; OnTimeout?.Invoke(); }
            }
            ui.UpdateTimerDisplay(TimeRemaining); // requires 'ui' reference
        }
    }

    public void AddCatch()
    {
        FishCount += 1;
        TimeRemaining = Mathf.Min(TimeRemaining + bonusTimePerFish, startTime + maxTimeCapExtra);
        if (TimeRemaining > 0f) { SetTimerColor(Color.white); firedTimeout = false; }
        ui.UpdateFishCounterDisplay(FishCount);
        ui.UpdateTimerDisplay(TimeRemaining);
    }

    // --- Helpers ---
    void SetTimerColor(Color c)
    {
        if (timerText) timerText.color = c;
    }
}
