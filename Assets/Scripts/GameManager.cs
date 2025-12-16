using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Allow to call GameManager.Instance anywhere (singleton)
    public static GameManager Instance { get; private set; }

    // World content
    [SerializeField]    
    private TimeOfDayRegistry timeOfDayRegistry;

    [SerializeField]
    private MapRegistry mapRegistry;

    [SerializeField]
    private PlayerEquipmentRegistry playerEquipmentRegistry;

    [SerializeField]
    private IngredientRegistry ingredientRegistry;

    [SerializeField]
    private RecipeRegistry recipeRegistry;

    [SerializeField]
    private FishRegistry fishRegistry;

    [SerializeField]
    private TransitionRegistry transitionRegistry;

    // Attributes
    private MapSO currentMap;
    private TimeOfDaySO currentTimeOfDay;
    private TransitionSO currentTransition;
    private float timeRemaining;
    private int daysCount;
    private int nightsCount;
    private bool isFirstDay;
    private bool isFirstNight;
    private bool isFishingTutorialEnabled;
    private bool isMapSelectionExplanationEnabled;
    private bool isRecipeBookUnlocked;

    // READ-ONLY ATTRIBUTES, CAN BE READ ANYWHERE
    public TimeOfDayRegistry TimeOfDayRegistry => timeOfDayRegistry;
    public MapRegistry MapRegistry => mapRegistry;
    public PlayerEquipmentRegistry PlayerEquipmentRegistry => playerEquipmentRegistry;
    public IngredientRegistry IngredientRegistry => ingredientRegistry;
    public RecipeRegistry RecipeRegistry => recipeRegistry;
    public FishRegistry FishRegistry => fishRegistry;
    public TransitionRegistry TransitionRegistry => transitionRegistry;
    public float TimeRemaining => timeRemaining;
    public MapSO CurrentMap => currentMap;
    public TimeOfDaySO CurrentTimeOfDay => currentTimeOfDay;
    public TransitionSO CurrentTransition => currentTransition;
    public int DaysCount => daysCount;
    public int NightsCount => nightsCount;
    public bool IsFirstDay => isFirstDay;
    public bool IsFirstNight => isFirstNight;
    public bool IsFishingTutorialEnabled => isFishingTutorialEnabled;
    public bool IsMapSelectionExplanationEnabled => isMapSelectionExplanationEnabled;
    public bool IsRecipeBookUnlocked => isRecipeBookUnlocked;

    // Internal attributes    
    private Dictionary<IngredientSO, int> obtainedIngredientLastDayAndNight;

    // Internal game states
    private enum GameState
    {
        Main,
        TransitionView,
        MapSelection,
        InventoryView,
        FishingView,
        MonsterView,
        IntroEvent,
        RecipeBookEvent,
        EndEvent
    }
    private GameState lastState;
    private GameState currentState;

    // Make this class a singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep the game context active for all scenes
        DontDestroyOnLoad(gameObject);
    }

    // Start the game mecanisms loop (called when the game start)
    private void Start()
    {        
        // Initialize registers
        timeOfDayRegistry.Initialize();
        mapRegistry.Initialize();
        playerEquipmentRegistry.Initialize();
        ingredientRegistry.Initialize();
        recipeRegistry.Initialize(); // RecipeRegistry must be initialized after playerEquipmentRegistry and ingredientsRegistry
        fishRegistry.Initialize();
        transitionRegistry.Initialize();

        // Initialize attributes
        currentTimeOfDay = TimeOfDayRegistry.daySO;
        currentMap = MapRegistry.driftwoodRiverSO; // Default map, for the first day of fishing
        daysCount = 1;
        nightsCount = 0;
        isFirstDay = true;
        isFirstNight = false;
        isFishingTutorialEnabled = true;
        isMapSelectionExplanationEnabled = true;
        isRecipeBookUnlocked = true; // TO CHANGE : set at false when recipe book event made
        InitializeObtainedIngredientsLastDayAndNight();
    }


    // NAVIGATION FONCTIONS

    // Start Game
    public void OnStartButtonPressed()
    {
        // TO CHANGE BY WHEN THE INTRO EVENT MADE : ChangeState(GameState.IntroEvent);
        StartTimer();
        ChangeState(GameState.FishingView);
    }

    // Quit game
    public void OnExitButtonPressed()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Called at the end of the introEvent
    public void ExitIntroEvent()
    {
        StartTimer();
        ChangeState(GameState.FishingView);
    }

    // Called at the end of the recipeBookEvent
    public void ExitRecipeBookEvent()
    {
        isRecipeBookUnlocked = true;
        currentTransition = transitionRegistry.endRecipeBookEventSO;
        ChangeState(GameState.TransitionView); // Next state is in the ExitTransition function
    }

    // Called in the FishingView at the end of the tutorial, unblock the timer
    public void EndOfFishingTutorial()
    {
        isFishingTutorialEnabled = false;
    }

    // Called in the MapSelection Scene when clicking on a map
    public void SelectMap(MapSO mapSelected)
    {
        currentMap = mapSelected;
        StartTimer();
        ChangeState(GameState.FishingView);
    }

    // Called in the MapSelection at the end of the explanations
    public void EndOfMapSelectionExplanation()
    {
        isMapSelectionExplanationEnabled = false;
    }

    // Called when clicking on the inventory button
    public void EnterInventory()
    {
        ChangeState(GameState.InventoryView);
    }

    // Called in the Inventory Scene when clicking Exit
    public void ExitInventory()
    {
        if (lastState == GameState.MapSelection)
        {
            ChangeState(GameState.MapSelection);   
        }
        else if (lastState == GameState.FishingView)
        {
            ChangeState(GameState.FishingView);
        }
    }

    // Called in the FishingView Scene in need of passing in monster view
    public void EnterMonsterView()
    {
        ChangeState(GameState.MonsterView);
    }

    // Called in the MonsterView Scene after winning
    public void WinAgainstMonster()
    {
        if (isFirstNight)
        {
            ChangeCurrentTimeOfDay();
            ChangeState(GameState.MapSelection); // TO CHANGE BY WHEN THE RECIPE BOOK EVENT MADE : add ChangeState(GameState.RecipeBookEvent);
        }
        else
        {
            ChangeState(GameState.FishingView); // The other nights, we go back to fishing
        }
    }

    // Called in the MonsterView Scene after dying
    public void DeathAgainstMonster()
    {
        if (isFirstNight)
        {
            currentTransition = transitionRegistry.firstDeathAgainstMonsterSO;
            ChangeState(GameState.TransitionView); // Next state is in the ExitTransition function
        }
        else
        {
            // The player loses what he obtained last day and that night
            foreach (var ingredient in ingredientRegistry.AllIngredients)
            {
                RemoveIngredient(ingredient, obtainedIngredientLastDayAndNight[ingredient]);
            }

            currentTransition = transitionRegistry.deathAgainstMonsterSO;
            ChangeState(GameState.TransitionView); // Next state is in the ExitTransition function

            // The next day start
            ChangeCurrentTimeOfDay();
        }
    }

    // Called when the time is out, we exit the fishing/monster view and return to the map selection
    private void TimeOut()
    {
        if (currentTimeOfDay == timeOfDayRegistry.daySO)
        {
            currentTransition = transitionRegistry.endDaySO;
            ChangeState(GameState.TransitionView); // Next state is in the ExitTransition function
        }
        else
        {            
            currentTransition = transitionRegistry.endNightSO;
            ChangeState(GameState.TransitionView); // Next state is in the ExitTransition function
        }

        // Change the time of day
        ChangeCurrentTimeOfDay();
    }

    // Called at the end of a transition, go to the next state
    public void ExitTransition()
    {
        if (currentTransition == transitionRegistry.endDaySO)
        {
            ChangeState(GameState.MapSelection);
        }
        else if (currentTransition == transitionRegistry.endNightSO)
        {
            ChangeState(GameState.MapSelection);   
        }
        else if (currentTransition == transitionRegistry.firstDeathAgainstMonsterSO)
        {
            ChangeState(GameState.MonsterView);
        }
        else if (currentTransition == transitionRegistry.deathAgainstMonsterSO)
        {
            ChangeState(GameState.MapSelection);
        }
        else if (currentTransition == transitionRegistry.endRecipeBookEventSO)
        {
            ChangeState(GameState.MapSelection);
        }
    }

    // Called in the InventoryView when the player make the final remedy
    public void EnterEndEvent()
    {
        ChangeState(GameState.EndEvent);
    }


    // INVENTORY & EQUIPMENT FUNCTIONS

    // Called to add ingredient to inventory and in the temporary ingredients dictionary
    public void AddIngredient(IngredientSO ingredient, int amount)
    {
        ingredient.playerQuantityPossessed += amount;
        obtainedIngredientLastDayAndNight[ingredient] += amount;
    }

    // Called when removing ingredient from inventory, return true if possible, false otherwise
    public bool RemoveIngredient(IngredientSO ingredient, int amount)
    {
        if (ingredient.playerQuantityPossessed < amount)
        {
            return false;
        }

        ingredient.playerQuantityPossessed -= amount;

        return true;
    }

    // Called when upgrading equipment
    public void UpgradeEquipment(PlayerEquipmentSO playerEquipment, int newLevel)
    {
        playerEquipment.UpgradeTo(newLevel);
    }


    // HELPING FONCTIONS

    // Update the game (called at each frame of the game)
    private void Update()
    {
        // Handle the game update logic for states
        switch (currentState)
        {
            case GameState.Main:
                break;
            
            case GameState.TransitionView:
                break;

            case GameState.MapSelection:
                break;

            case GameState.InventoryView:
                break;

            case GameState.FishingView:
                if (!IsFishingTutorialEnabled) { UpdateTimerAndCheckIfOut(); } // No timer during fishing tutorial
                break;

            case GameState.MonsterView:
                if (!IsFirstNight) { UpdateTimerAndCheckIfOut(); } // No timer during monster tutorial
                break;

            case GameState.IntroEvent:
                break;

            case GameState.RecipeBookEvent:
                break;

            case GameState.EndEvent:
                break;
        }
    }

    // Pass from one state to another
    private void ChangeState(GameState newState)
    {
        lastState = currentState;
        currentState = newState;

        switch (currentState)
        {
            case GameState.Main:
                SceneManager.LoadScene("Main");
                break;
            case GameState.TransitionView:
                SceneManager.LoadScene("TransitionView");
                break;
            case GameState.MapSelection:
                SceneManager.LoadScene("MapSelection");
                break;
            case GameState.InventoryView:
                SceneManager.LoadScene("InventoryView");
                break;
            case GameState.FishingView:
                SceneManager.LoadScene("FishingView");
                break;
            case GameState.MonsterView:
                SceneManager.LoadScene("MonsterView");
                break;
            case GameState.IntroEvent:
                SceneManager.LoadScene("IntroEvent");
                break;
            case GameState.RecipeBookEvent:
                SceneManager.LoadScene("RecipeBookEvent");
                break;
            case GameState.EndEvent:
                SceneManager.LoadScene("EndEvent");
                break;
        }
    }

    private void StartTimer()
    {
        if (currentTimeOfDay == TimeOfDayRegistry.daySO)
        {
            timeRemaining = TimeOfDayRegistry.daySO.duration;
        }
        else
        {
            timeRemaining = TimeOfDayRegistry.nightSO.duration;
        }
    }

    private void UpdateTimerAndCheckIfOut()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            TimeOut();
        }
    }

    private void ChangeCurrentTimeOfDay()
    {
        if (currentTimeOfDay == TimeOfDayRegistry.daySO)
        {
            currentTimeOfDay = TimeOfDayRegistry.nightSO;
            nightsCount++;
            isFirstDay = false;
            if (nightsCount == 1)
            {
                isFirstNight = true;
            }
            else
            {
                isFirstNight = false;
            }
        }
        else
        {
            InitializeObtainedIngredientsLastDayAndNight();
            currentTimeOfDay = TimeOfDayRegistry.daySO;
            daysCount++;
            isFirstNight = false;
            if (daysCount == 1)
            {
                isFirstDay = true;
            }
            else
            {
                isFirstDay = false;
            }
        }
    }

    private void InitializeObtainedIngredientsLastDayAndNight()
    {
        obtainedIngredientLastDayAndNight = new Dictionary<IngredientSO, int>();
        foreach (var ingredient in ingredientRegistry.AllIngredients)
        {
            obtainedIngredientLastDayAndNight[ingredient] = 0;
        }
    }
}
