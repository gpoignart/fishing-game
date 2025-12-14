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

    // Parameters
    [SerializeField]
    private int monsterSpawnChance = 70;

    [SerializeField]
    private float monsterSpawnCheckInterval = 10f;

    // Internal attributes
    private MapSO currentMap;
    private TimeOfDaySO currentTimeOfDay;
    private TransitionSO currentTransition;
    private float timeRemaining;
    private int daysCount;
    private int nightsCount;
    private bool isFirstDay;
    private bool isFirstNight;
    private bool isRecipeBookUnlocked;
    private Dictionary<IngredientSO, int> obtainedIngredientLastDayAndNight;

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
    public bool IsRecipeBookUnlocked => isRecipeBookUnlocked;
    public int MonsterSpawnChance => monsterSpawnChance;
    public float MonsterSpawnCheckInterval => monsterSpawnCheckInterval;

    // Game states
    public enum GameState
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
        recipeRegistry.Initialize();
        fishRegistry.Initialize();
        transitionRegistry.Initialize();

        // Initialize attributes
        currentTimeOfDay = TimeOfDayRegistry.daySO;
        daysCount = 1;
        nightsCount = 0;
        isFirstDay = true;
        isFirstNight = false;
        isRecipeBookUnlocked = true; // TO CHANGE : set at false when recipe book event made
        InitializeObtainedIngredientsLastDayAndNight();
    }


    // NAVIGATION FONCTIONS

    // Start Game
    public void OnStartButtonPressed()
    {
        // TO CHANGE BY WHEN THE INTRO EVENT MADE : ChangeState(GameState.IntroEvent);
        ChangeState(GameState.MapSelection);
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
        ChangeState(GameState.MapSelection);
    }

    // Called at the end of the recipeBookEvent
    public void ExitRecipeBookEvent()
    {
        ChangeState(GameState.MapSelection);
        isRecipeBookUnlocked = true;
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

    // Called in the MapSelection Scene when clicking on a map
    public void SelectMap(MapSO mapSelected)
    {
        currentMap = mapSelected;
        StartTimer();
        ChangeState(GameState.FishingView);
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
            ChangeState(GameState.TransitionView); // Transition manager will change state to MonsterView to redoing the fight because it's first night
        }
        else
        {
            // The player loses what he obtained last day and that night
            foreach (var ingredient in ingredientRegistry.AllIngredients)
            {
                RemoveIngredient(ingredient, obtainedIngredientLastDayAndNight[ingredient]);
            }

            currentTransition = transitionRegistry.deathAgainstMonsterSO;
            ChangeState(GameState.TransitionView); // Transition manager will change state to Map Selection

            // The next day start
            ChangeCurrentTimeOfDay();
        }
    }

    // Called when the time is out, we exit the fishing/monster view and return to the map selection
    private void TimeOut()
    {
        if (isFirstNight) { return; } // The first night is not influenced by the timer as it's the monster tutorial
        
        if (currentTimeOfDay == timeOfDayRegistry.daySO)
        {
            currentTransition = transitionRegistry.endDaySO;
            ChangeState(GameState.TransitionView); // Transition manager will change state to Map Selection
        }
        else
        {            
            currentTransition = transitionRegistry.endNightSO;
            ChangeState(GameState.TransitionView); // Transition manager will change state to Map Selection 
        }

        // Change the time of day
        ChangeCurrentTimeOfDay();
    }

    // Called in the InventoryView when the player make the final remedy
    public void EnterEndEvent()
    {
        ChangeState(GameState.EndEvent);
    }


    // INVENTORY FUNCTIONS

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
    public void UpgradeEquipment(PlayerEquipmentSO playerEquipment)
    {
        playerEquipment.level++;
    }


    // HELPING FONCTIONS

    // Update the game (called at each frame of the game)
    void Update()
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
                UpdateTimerAndCheckIfOut();
                break;

            case GameState.MonsterView:
                UpdateTimerAndCheckIfOut();
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
    public void ChangeState(GameState newState)
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
