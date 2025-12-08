using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingGameManager : MonoBehaviour
{
    // Allow to call FishingGameManager.Instance anywhere (singleton)
    public static FishingGameManager Instance { get; private set; }

    // Animator
    [SerializeField] private Animator playerAnimator;

    // Internal states
    private enum FishingGameState
    {
        Moving,
        Hooking,
        Fishing
    }
    private FishingGameState currentState;

    // Internal parameters
    private Fish currentFishBelow = null;
    private IngredientSO pendingLoot = null;

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

    // Start the game mecanisms loop (called when the fishing view is loaded)
    void Start()
    {
        FishingUIManager.Instance.UpdateTimerUI(GameManager.Instance.TimeRemaining);
        FishingUIManager.Instance.HideLoot();

        // Start the monster spawn loop at night
        if (GameManager.Instance.CurrentTimeOfDay == GameManager.Instance.TimeOfDayRegistry.nightSO) { StartCoroutine(MonsterSpawnLoop()); }

        // First state
        ChangeState(FishingGameState.Moving);
    }

    // Update the game (called at each frame of the game in fishing view)
    void Update()
    {
        // Update the timer
        FishingUIManager.Instance.UpdateTimerUI(GameManager.Instance.TimeRemaining);

        // Handle the game update logic for states
        switch (currentState)
        {
            case FishingGameState.Moving:
                PlayerController.Instance.UpdatePlayer();
                break;

            case FishingGameState.Hooking:
                PlayerController.Instance.UpdatePlayer();
                break;

            case FishingGameState.Fishing:
                FishingMinigameManager.Instance.UpdateMiniGame();
                break;
        }
    }

    // All the monsterSpawnCheckInterval, try to spawn a monster with a monsterSpawnChance
    private IEnumerator MonsterSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.Instance.MonsterSpawnCheckInterval);

            // The first night, the tutorial monster appears always after the first interval
            if (GameManager.Instance.IsFirstNight)
            {
                GameManager.Instance.EnterMonsterView();
            }
            // The other nights, a monster can appear with a monsterSpawnChance at each interval
            else if (Random.Range(0, 100) <= GameManager.Instance.MonsterSpawnChance)
            {
                GameManager.Instance.EnterMonsterView();
            }
        }
    }

    // Pass from one state to another
    private void ChangeState(FishingGameState newState)
    {
        // Exit logic for the previous state
        OnStateExit(currentState);

        currentState = newState;

        // Enter logic for the new state
        OnStateEnter(currentState);
    }

    // Handle the game logic when entering states
    private void OnStateEnter(FishingGameState state)
    {
        switch (state)
        {
            case FishingGameState.Moving:
                Debug.Log("Entering Moving state");
                break;

            case FishingGameState.Hooking:
                FishingUIManager.Instance.ShowHookingStateUI();
                Debug.Log("Entering Hooking state");
                break;

            case FishingGameState.Fishing:
                FishingUIManager.Instance.ShowFishingStateUI();
                FishingMinigameManager.Instance.StartMiniGame(currentFishBelow);
                Debug.Log("Entering Fishing state");
                break;
        }
    }

    // Handle the game logic when exiting states
    private void OnStateExit(FishingGameState state)
    {
        switch (state)
        {
            case FishingGameState.Moving:
                Debug.Log("Exiting Moving state");
                break;

            case FishingGameState.Hooking:
                FishingUIManager.Instance.HideHookingStateUI();
                Debug.Log("Exiting Hooking state");
                break;

            case FishingGameState.Fishing:
                FishingUIManager.Instance.HideFishingStateUI();
                Debug.Log("Exiting Fishing state");
                break;
        }
    }

    // Called by the PlayerController if the player is above a fish, keep the fish in memory
    public void PlayerAboveFish(Fish fishBelow)
    {
        if (currentState != FishingGameState.Hooking)
        {
            ChangeState(FishingGameState.Hooking);
        }
        currentFishBelow = fishBelow;
    }

    // Called by the PlayerController if the player is not above a fish, no fish in memory
    public void PlayerNotAboveFish()
    {
        if (currentState != FishingGameState.Moving)
        {
            ChangeState(FishingGameState.Moving);
        }
        currentFishBelow = null;
    }

    // Called when the player clicks the Hook button
    public void OnHookButtonPressed()
    {
        ChangeState(FishingGameState.Fishing);
    }

    // Called by the FishingMinigameManager when success
    public void FishingMinigameSuccess()
    {
        ChangeState(FishingGameState.Moving);

        // Trigger player animation
        playerAnimator.SetTrigger("OnCatch");

        // Obtain the ingredient
        pendingLoot = currentFishBelow.fishSO.drops[Random.Range(0, currentFishBelow.fishSO.drops.Length)];
        GameManager.Instance.AddIngredient(pendingLoot, 1);

        // Delete the fish
        Destroy(currentFishBelow.gameObject);
        currentFishBelow = null;
    }

    // Called by the FishingMinigameManager when fail
    public void FishingMinigameFail()
    {
        ChangeState(FishingGameState.Moving);

        // Delete the fish
        Destroy(currentFishBelow.gameObject);
        currentFishBelow = null;
    }

    // Show the loot
    public void ShowLoot()
    {
        FishingUIManager.Instance.ShowLoot(pendingLoot);
    }

    // Hide the loot
    public void HideLoot()
    {
        FishingUIManager.Instance.HideLoot();
    }
}
