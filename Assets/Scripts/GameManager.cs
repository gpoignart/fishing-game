using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Allow to call GameManager.Instance anywhere (singleton)
    public static GameManager Instance { get; private set; }

    // World content
    [SerializeField] private TimeOfDayRegistry timeOfDayRegistry;
    [SerializeField] private MapRegistry mapRegistry;
    [SerializeField] private PlayerEquipmentRegistry playerEquipmentRegistry;
    [SerializeField] private IngredientRegistry ingredientRegistry;
    [SerializeField] private RecipeRegistry recipeRegistry;
    [SerializeField] private FishRegistry fishRegistry;

    // Monster apparition
    [SerializeField] private int monsterSpawnChance = 70;
    [SerializeField] private float monsterSpawnCheckInterval = 10f;

    // Internal attributes
    private MapSO currentMap;
    private TimeOfDaySO currentTimeOfDay;
    private float timeRemaining;
    private int daysCount;
    private int nightsCount;
    private bool isFirstDay;
    private bool isFirstNight;
    private bool isRecipeBookUnlocked;

    // READ-ONLY ATTRIBUTES, CAN BE READ ANYWHERE
    public TimeOfDayRegistry TimeOfDayRegistry => timeOfDayRegistry;
    public MapRegistry MapRegistry => mapRegistry;
    public PlayerEquipmentRegistry PlayerEquipmentRegistry => playerEquipmentRegistry;
    public IngredientRegistry IngredientRegistry => ingredientRegistry;
    public RecipeRegistry RecipeRegistry => recipeRegistry;
    public FishRegistry FishRegistry => fishRegistry;
    public float TimeRemaining => timeRemaining;
    public MapSO CurrentMap => currentMap;
    public TimeOfDaySO CurrentTimeOfDay => currentTimeOfDay;
    public int DaysCount => daysCount;
    public int NightsCount => nightsCount;
    public bool IsFirstDay => isFirstDay;
    public bool IsFirstNight => isFirstNight;
    public bool IsRecipeBookUnlocked => isRecipeBookUnlocked;
    public int MonsterSpawnChance => monsterSpawnChance;
    public float MonsterSpawnCheckInterval => monsterSpawnCheckInterval;

    // Internal states
    private enum GameState
    {
        MapSelection,
        InventoryView,
        FishingView,
        MonsterView,
        IntroEvent,
        RecipeBookEvent,
        EndEvent
    }
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
    void Start()
    {
        // Initialize registers
        timeOfDayRegistry.Initialize();
        mapRegistry.Initialize();
        playerEquipmentRegistry.Initialize();
        ingredientRegistry.Initialize();
        recipeRegistry.Initialize();
        fishRegistry.Initialize();

        // Initialize attributes
        currentTimeOfDay = TimeOfDayRegistry.daySO;
        daysCount = 1;
        nightsCount = 0;
        isFirstDay = true;
        isFirstNight = false;
        isRecipeBookUnlocked = true; // TO CHANGE : set at false when recipe book event made

        // First state
        // TO CHANGE BY WHEN THE INTRO EVENT MADE : ChangeState(GameState.IntroEvent);
        ChangeState(GameState.MapSelection);
    }


    // PUBLIC FONCTIONS

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

    // Called in the MapSelection Scene when clicking on the inventory
    public void EnterInventory()
    {
        ChangeState(GameState.InventoryView);
    }

    // Called in the Inventory Scene when clicking return
    public void ExitInventory()
    {
        ChangeState(GameState.MapSelection);
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
        Debug.Log("Win against monster");
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
            ChangeState(GameState.MonsterView); // The first night, we redo the fight in case of lose
        }
        else
        {
            ChangeState(GameState.MapSelection); // The other nights, we go back at the begining of the night
        }
    }

    // Called in the InventoryView when the player make the final remedy
    public void EnterEndEvent()
    {
        ChangeState(GameState.EndEvent);
    }

    // Called to add ingredient to inventory
    public void AddIngredient(IngredientSO ingredient, int amount)
    {
        ingredient.playerQuantityPossessed += amount;
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
        playerEquipment.level ++;
    }


    // PRIVATE FONCTIONS

    // Update the game (called at each frame of the game)
    void Update()
    {
        // Handle the game update logic for states
        switch (currentState)
        {
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
    private void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
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

    // Called when the time is out, we exit the fishing/monster view and return to the map selection
    private void TimeOut()
    {
        if (isFirstNight) { return; } // The first night is not influenced by the timer as it's the monster tutorial
        ChangeCurrentTimeOfDay();
        ChangeState(GameState.MapSelection);
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
            if (nightsCount == 1) { isFirstNight = true; }
            else { isFirstNight = false; }
        }
        else
        {
            currentTimeOfDay = TimeOfDayRegistry.daySO;
            daysCount++;
            isFirstNight = false;
            if (daysCount == 1) { isFirstDay = true; }
            else { isFirstDay = false; }
        }
    }
}
