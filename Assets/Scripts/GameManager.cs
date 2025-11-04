using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- References ---
    public UIManager uiManager;
    public DragMinigame minigame;     // Your drag minigame script
    public GameSession session;       // Timer + fish


    // --- State ---
    bool isRunning;

    void Start()
    {
        // Init HUD from session
        session.Reset();
        isRunning = true;
    } 
    void Update()
    {
        if (!isRunning) return;

        // tick timer and refresh HUD
        session.UpdateTimer(Time.deltaTime);
        uiManager.UpdateTimerDisplay(session.TimeRemaining);

        // stop when time reaches zero 
        if (session.TimeRemaining <= 0f) isRunning = false;
    }
    

    void OnDragSuccess()
    {
        // +1 fish, +time in Session; then refresh HUD and return to Idle
        session.AddCatch();
        uiManager.UpdateFishCounterDisplay(session.FishCount);
        uiManager.UpdateTimerDisplay(session.TimeRemaining);

    }

    void OnDragFail()
    {
        // No bonus; just return to Idle
        // stop timer at Idle
    }
}
