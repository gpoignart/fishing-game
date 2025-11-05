using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // References
    public UIManager uiManager;
    public DragMinigame minigame; // Your drag minigame script
    public GameSession session; // Timer + fish

    // States
    private bool gameStarted = false; // Is the game started
    private bool miniGameStarted = false; // Is the drag mini game started
    private bool hookPhaseActive = false; // Is the hook phrase is active

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Called when the game start, start the game mecanisms loop
    void Start()
    {
        // Init HUD from session
        session.Reset();

        // Show the idle screen first
        uiManager.ShowIdlePanel();
    }

    // Called at each frame of the game
    void Update()
    {
        // Only update the timer if the game has started
        if (gameStarted)
        {
            session.UpdateTimer(Time.deltaTime);
        }

        // Only update the minigame is the minigame has started
        if (miniGameStarted)
        {
            minigame.UpdateMiniGame();
        }
    }

    // Called when the player clicks the Start button
    public void OnStartButtonPressed()
    {
        // Start the actual game loop
        gameStarted = true;

        // Go to the wait screen
        Instance.OnWaitScreen();
    }

    // Handle the wait screen
    public void OnWaitScreen()
    {
        // Show the wait screen
        uiManager.ShowWaitPanel();

        // Start a coroutine to handle the waiting phase
        StartCoroutine(WaitBeforeHook());
    }

    // Coroutine that waits a few seconds before showing the hook screen
    private IEnumerator WaitBeforeHook()
    {
        // Wait between 1 and 3 seconds
        float waitTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(waitTime);

        // Hook phase
        hookPhaseActive = true;
        uiManager.ShowHookPanel();

        // The hook phase only last between 1 and 2 seconds
        float hookDuration = Random.Range(1f, 2f);
        yield return new WaitForSeconds(hookDuration);

        // If the player haven't click (= if the hook phase is again active)
        if (hookPhaseActive)
        {
            uiManager.ShowWaitPanel();
            hookPhaseActive = false;

            // We restart the coroutine
            StartCoroutine(WaitBeforeHook());
        }
    }

    // Called when the player clicks the Hook button
    public void OnHookButtonPressed()
    {
        // The player has clicked so we desactive the hook phase
        hookPhaseActive = false;

        // Show the drag screen
        uiManager.ShowDragPanel();

        // Start the mini-game
        minigame.StartMiniGame();

        // Update the mini game state
        miniGameStarted = true;
    }

    // Called when DragMinigam invokes OnDragSuccess
    public void OnDragSuccess()
    {
        // Add a catch
        session.AddCatch();

        // Return to the wait screen
        Instance.OnWaitScreen();
    }

    // Called when DragMinigame invokes OnDragFail
    public void OnDragFail()
    {
        // Stop the minigame
        miniGameStarted = false;

        // Return to the wait screen
        Instance.OnWaitScreen();
    }

    // Called when GameSession invokes OnTimeout
    public void OnTimeout()
    {
        // Stop the game and the minigame
        gameStarted = false;
        miniGameStarted = false;
        StopAllCoroutines();

        // Go to the timeout screen
        uiManager.ShowTimeOutPanel();
    }

    // Called when the player clicks the replay button
    public void OnReplayButtonPressed()
    {
        // Initialize game's states
        miniGameStarted = false;
        hookPhaseActive = false;
        gameStarted = true;

        // Reinitialize the HUD from session
        session.Reset();

        // We goes to the wait Screen
        Instance.OnWaitScreen();
    }

    // Called when the player clicks the exit button
    public void OnExitButtonPressed()
    {
        // Quit the game if running inside the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application in a built version
        Application.Quit();
#endif
    }
}
