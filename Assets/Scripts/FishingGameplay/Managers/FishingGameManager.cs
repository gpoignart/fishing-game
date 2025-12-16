using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingGameManager : MonoBehaviour
{
    // Allow to call FishingGameManager.Instance anywhere (singleton)
    public static FishingGameManager Instance { get; private set; }


    // Parameters
    private int monsterSpawnChance = 50;
    private float monsterSpawnCheckInterval = 10f;

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


    // Internal references
    private Fish currentFishBelow = null;
    private IngredientSO pendingLoot = null;


    // Tutorial states
    private enum FishingTutorialState
    {
        Start,
        Move,
        Flip,
        DetectFish,
        Hook,
        Fishing,
        Loot,
        Inventory,
        Timer,
        End
    }
    private FishingTutorialState currentTutorialState = FishingTutorialState.Start;
    private bool tutorialFishHasSpawned;
    

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
    private void Start()
    {
        // Tutorial initialization
        if (GameManager.Instance.IsFishingTutorialEnabled)
        {
            ChangeTutorialState(FishingTutorialState.Move); // Begin tutorial
        }

        // Start the fish spawner if not in tutorial
        if (!GameManager.Instance.IsFishingTutorialEnabled) { FishSpawner.Instance.StartFishSpawner(); }

        // Start the monster spawn loop at night
        if (GameManager.Instance.CurrentTimeOfDay == GameManager.Instance.TimeOfDayRegistry.nightSO) { StartCoroutine(MonsterSpawnLoop()); }

        // First state
        ChangeState(FishingGameState.Moving);
    }

    // Update the game (called at each frame of the game in fishing view)
    private void Update()
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
                if (Input.GetKeyDown(KeyCode.Space)) { OnHookButtonPressed(); } // Click on hook button with space
                break;

            case FishingGameState.Fishing:
                FishingMinigameManager.Instance.UpdateMiniGame();
                break;
        }

        // Handle the game update logic for tutorial states
        switch (currentTutorialState)
        {
            case FishingTutorialState.Loot:
                if (Input.GetKeyDown(KeyCode.Return)) { OnTutorialNextButtonPressed(); } // Click on next button with return
                break;

            case FishingTutorialState.Inventory:
                if (Input.GetKeyDown(KeyCode.Return)) { OnTutorialNextButtonPressed(); } // Click on next button with return
                break;

            case FishingTutorialState.Timer:
                if (Input.GetKeyDown(KeyCode.Return)) { OnTutorialNextButtonPressed(); } // Click on next button with return
                break;
        }
    }

    // All the monsterSpawnCheckInterval, try to spawn a monster with a monsterSpawnChance
    private IEnumerator MonsterSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(monsterSpawnCheckInterval);

            // The first night, the tutorial monster appears always after the first interval
            if (GameManager.Instance.IsFirstNight)
            {
                GameManager.Instance.EnterMonsterView();
            }
            // The other nights, a monster can appear with a monsterSpawnChance at each interval
            else if (Random.Range(0, 100) <= monsterSpawnChance)
            {
                GameManager.Instance.EnterMonsterView();
            }
        }
    }


    // Called by the PlayerController if the player has moved
    public void PlayerHasMoved()
    {
        if (GameManager.Instance.IsFishingTutorialEnabled && currentTutorialState == FishingTutorialState.Move)
        {
            ChangeTutorialState(FishingTutorialState.Flip);
        }
    }

    // Called by the PlayerController if the player has flipped
    public void PlayerHasFlipped()
    {
        if (GameManager.Instance.IsFishingTutorialEnabled && currentTutorialState == FishingTutorialState.Flip)
        {
            ChangeTutorialState(FishingTutorialState.DetectFish);
        }
    }

    // Check if the player is allowed to flip
    public bool CanPlayerFlip()
    {
        return !GameManager.Instance.IsFishingTutorialEnabled || currentTutorialState >= FishingTutorialState.Flip;
    }

    // Called by the PlayerController if the player is above a fish, keep the fish in memory
    public void PlayerAboveFish(Fish fishBelow)
    {
        if (GameManager.Instance.IsFishingTutorialEnabled && currentTutorialState < FishingTutorialState.DetectFish) { return; }
        if (GameManager.Instance.IsFishingTutorialEnabled && currentTutorialState == FishingTutorialState.DetectFish) { ChangeTutorialState(FishingTutorialState.Hook); }

        if (currentState != FishingGameState.Hooking)
        {
            ChangeState(FishingGameState.Hooking);
        }
        currentFishBelow = fishBelow;
    }

    // Called by the PlayerController if the player is not above a fish, no fish in memory
    public void PlayerNotAboveFish()
    {
        if (GameManager.Instance.IsFishingTutorialEnabled && currentTutorialState == FishingTutorialState.Hook) { ChangeTutorialState(FishingTutorialState.DetectFish); }
        
        if (currentState != FishingGameState.Moving)
        {
            ChangeState(FishingGameState.Moving);
        }
        currentFishBelow = null;
    }

    // Called when the player clicks the Hook button
    public void OnHookButtonPressed()
    {
        if (GameManager.Instance.IsFishingTutorialEnabled && currentTutorialState < FishingTutorialState.Hook) { return; }
        if (GameManager.Instance.IsFishingTutorialEnabled && currentTutorialState == FishingTutorialState.Hook) { ChangeTutorialState(FishingTutorialState.Fishing); }

        ChangeState(FishingGameState.Fishing);
    }

    // Called when the player clicks inventory button
    public void OnInventoryButtonPressed()
    {
        if (GameManager.Instance.IsFishingTutorialEnabled) { return; }

        GameManager.Instance.EnterInventory();
    }

    // Called when the player clicks extend commands button
    public void OnExtendCommandsButtonPressed()
    {
        if (GameManager.Instance.IsFishingTutorialEnabled) { return; }

        FishingUIManager.Instance.HideExtendCommandsButton();
        FishingUIManager.Instance.ShowCollapseCommandsButton();
        FishingUIManager.Instance.ShowExtendedCommands();
    }

    // Called when the player clicks collapse commands button
    public void OnCollapseCommandsButtonPressed()
    {
        if (GameManager.Instance.IsFishingTutorialEnabled) { return; }

        FishingUIManager.Instance.ShowExtendCommandsButton();
        FishingUIManager.Instance.HideCollapseCommandsButton();
        FishingUIManager.Instance.HideExtendedCommands();
    }

    // Called by the FishingMinigameManager when success
    public void FishingMinigameSuccess()
    {
        if (GameManager.Instance.IsFishingTutorialEnabled && currentTutorialState < FishingTutorialState.Fishing) { return; }

        ChangeState(FishingGameState.Moving);

        // Obtain the ingredient
        pendingLoot = currentFishBelow.fishSO.drops[Random.Range(0, currentFishBelow.fishSO.drops.Length)];
        GameManager.Instance.AddIngredient(pendingLoot, 1);

        // Trigger player animation
        playerAnimator.SetTrigger("OnCatch");

        // Show the loot for a few seconds if not in tutorial
        if (!GameManager.Instance.IsFishingTutorialEnabled)
        {
            StartCoroutine(FishingUIManager.Instance.ShowLootForSeconds(pendingLoot, 1.5f));
        }
        // Change to loot if in tutorial state (must do after obtained the ingredient)
        else if (GameManager.Instance.IsFishingTutorialEnabled && currentTutorialState == FishingTutorialState.Fishing)
        {
            ChangeTutorialState(FishingTutorialState.Loot);
        }

        // Delete the fish
        Destroy(currentFishBelow.gameObject);
        currentFishBelow = null;
    }

    // Called by the FishingMinigameManager when fail
    public void FishingMinigameFail()
    {
        // In tutorial : we don't quit the minigame while it is not winned
        if (GameManager.Instance.IsFishingTutorialEnabled && currentTutorialState <= FishingTutorialState.Fishing) { return; }

        ChangeState(FishingGameState.Moving);

        // Show the loseFishText
        StartCoroutine(FishingUIManager.Instance.ShowLoseFishTextForSeconds(1.5f));

        // Delete the fish
        Destroy(currentFishBelow.gameObject);
        currentFishBelow = null;
    }

    public void OnTutorialNextButtonPressed()
    {
        if (!GameManager.Instance.IsFishingTutorialEnabled) { return; }
        if (currentTutorialState == FishingTutorialState.Loot)
        {
            ChangeTutorialState(FishingTutorialState.Inventory);
        }
        else if (currentTutorialState == FishingTutorialState.Inventory)
        {
            ChangeTutorialState(FishingTutorialState.Timer);
        }
        else if (currentTutorialState == FishingTutorialState.Timer)
        {
            TutorialFinished();
        }
    }

    // Called at the end of the endState of the tutorial, start the real day 1
    public void TutorialFinished()
    {
        ChangeTutorialState(FishingTutorialState.End);
        GameManager.Instance.EndOfFishingTutorial();
        FishSpawner.Instance.StartFishSpawner();
    }


    // HANDLE GAME STATES

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


    // HANDLE TUTORIAL STATES
    
    // Pass from one state to another
    private void ChangeTutorialState(FishingTutorialState newTutorialState)
    {
        // Exit logic for the previous state
        OnTutorialStateExit(currentTutorialState);

        currentTutorialState = newTutorialState;

        // Enter logic for the new state
        OnTutorialStateEnter(currentTutorialState);
    }

    // Handle the game logic when entering states
    private void OnTutorialStateEnter(FishingTutorialState state)
    {
        switch (state)
        {
            case FishingTutorialState.Start:
                break;

            case FishingTutorialState.Move:
                FishingTutorialUIManager.Instance.ShowMoveTutorialStepUI();
                break;

            case FishingTutorialState.Flip:
                FishingTutorialUIManager.Instance.ShowFlipTutorialStepUI();
                break;

            case FishingTutorialState.DetectFish:
                FishingTutorialUIManager.Instance.ShowDetectFishTutorialStepUI();
                if (!tutorialFishHasSpawned)
                {
                    FishSpawner.Instance.SpawnTutorialFish();
                    tutorialFishHasSpawned = true;
                }
                break;

            case FishingTutorialState.Hook:
                FishingTutorialUIManager.Instance.ShowHookTutorialStepUI();
                break;

            case FishingTutorialState.Fishing:
                FishingTutorialUIManager.Instance.ShowFishingTutorialStepUI();
                break;

            case FishingTutorialState.Loot:
                FishingTutorialUIManager.Instance.ShowLootTutorialStepUI();
                FishingUIManager.Instance.ShowLoot(pendingLoot);
                break;

            case FishingTutorialState.Inventory:
                FishingTutorialUIManager.Instance.ShowInventoryTutorialStepUI();
                FishingUIManager.Instance.ShowInventoryButton();
                break;

            case FishingTutorialState.Timer:
                FishingTutorialUIManager.Instance.ShowTimerTutorialStepUI();
                FishingUIManager.Instance.ShowInventoryButton();
                FishingUIManager.Instance.ShowTimer();
                break;
            
            case FishingTutorialState.End:
                FishingUIManager.Instance.ShowTimer();
                FishingUIManager.Instance.ShowInventoryButton();
                FishingUIManager.Instance.ShowCommandsPanel();
                FishingUIManager.Instance.HideTutorialPanel();
                break;
        }
    }

    // Handle the game logic when exiting states
    private void OnTutorialStateExit(FishingTutorialState state)
    {
        switch (state)
        {
            case FishingTutorialState.Start:
                tutorialFishHasSpawned = false;
                FishingUIManager.Instance.HideTimer();
                FishingUIManager.Instance.HideInventoryButton();
                FishingUIManager.Instance.HideCommandsPanel();
                break;

            case FishingTutorialState.Move:
                FishingTutorialUIManager.Instance.HideMoveTutorialStepUI();
                break;

            case FishingTutorialState.Flip:
                FishingTutorialUIManager.Instance.HideFlipTutorialStepUI();
                break;

            case FishingTutorialState.DetectFish:
                FishingTutorialUIManager.Instance.HideDetectFishTutorialStepUI();
                break;

            case FishingTutorialState.Hook:
                FishingTutorialUIManager.Instance.HideHookTutorialStepUI();
                break;

            case FishingTutorialState.Fishing:
                FishingTutorialUIManager.Instance.HideFishingTutorialStepUI();
                break;

            case FishingTutorialState.Loot:
                FishingTutorialUIManager.Instance.HideLootTutorialStepUI();
                FishingUIManager.Instance.HideLoot();
                break;

            case FishingTutorialState.Inventory:
                FishingTutorialUIManager.Instance.HideInventoryTutorialStepUI();
                FishingUIManager.Instance.HideInventoryButton();
                break;
            
            case FishingTutorialState.Timer:
                FishingTutorialUIManager.Instance.HideTimerTutorialStepUI();
                FishingUIManager.Instance.HideInventoryButton();
                FishingUIManager.Instance.HideTimer();
                break;

            case FishingTutorialState.End:
                break;
        }
    }
}
