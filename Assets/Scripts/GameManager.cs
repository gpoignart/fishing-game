using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

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
    private MonsterRegistry monsterRegistry;

    [SerializeField]
    private TransitionRegistry transitionRegistry;

    [SerializeField]
    private EventRegistry eventRegistry;

    // Attributes
    private MapSO currentMap;
    private TimeOfDaySO currentTimeOfDay;
    private TransitionSO currentTransition;
    private EventSO currentEvent;
    private float timeRemaining;
    private int daysCount;
    private int nightsCount;
    private bool isFirstDay;
    private bool isFirstNight;
    private bool isFishingTutorialEnabled;
    private bool isMapSelectionExplanationEnabled;
    private bool isRecipeBookUnlocked;
    private Vector2 fishingPlayerPosition;
    private Vector2 fishingPlayerOrientation;

    // READ-ONLY ATTRIBUTES, CAN BE READ ANYWHERE
    public TimeOfDayRegistry TimeOfDayRegistry => timeOfDayRegistry;
    public MapRegistry MapRegistry => mapRegistry;
    public PlayerEquipmentRegistry PlayerEquipmentRegistry => playerEquipmentRegistry;
    public IngredientRegistry IngredientRegistry => ingredientRegistry;
    public RecipeRegistry RecipeRegistry => recipeRegistry;
    public FishRegistry FishRegistry => fishRegistry;
    public MonsterRegistry MonsterRegistry => monsterRegistry;
    public TransitionRegistry TransitionRegistry => transitionRegistry;
    public EventRegistry EventRegistry => eventRegistry;
    public float TimeRemaining => timeRemaining;
    public MapSO CurrentMap => currentMap;
    public TimeOfDaySO CurrentTimeOfDay => currentTimeOfDay;
    public TransitionSO CurrentTransition => currentTransition;
    public EventSO CurrentEvent => currentEvent;
    public int DaysCount => daysCount;
    public int NightsCount => nightsCount;
    public bool IsFirstDay => isFirstDay;
    public bool IsFirstNight => isFirstNight;
    public bool IsFishingTutorialEnabled => isFishingTutorialEnabled;
    public bool IsMapSelectionExplanationEnabled => isMapSelectionExplanationEnabled;
    public bool IsRecipeBookUnlocked => isRecipeBookUnlocked;
    public Vector2 FishingPlayerPosition => fishingPlayerPosition;
    public Vector2 FishingPlayerOrientation => fishingPlayerOrientation;

    // Internal attributes    
    private Dictionary<IngredientSO, int> obtainedIngredientLastDayAndNight;
    private Vector2 defaultFishingPlayerPosition;
    private Vector2 defaultFishingPlayerOrientation;

    // Internal game views
    private enum GameView
    {
        Main,
        EventView,
        TransitionView,
        MapSelectionView,
        InventoryView,
        FishingView,
        MonsterView,
        EndScreenView
    }
    private GameView lastView;
    private GameView currentView;

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
        // First view
        ChangeView(GameView.Main);

        // Initialize internal references
        InitializeObtainedIngredientsLastDayAndNight();
        defaultFishingPlayerPosition = new Vector2(2f, 2.8f);
        defaultFishingPlayerOrientation = new Vector2(1f, 1f);

        // Initialize registers
        timeOfDayRegistry.Initialize();
        mapRegistry.Initialize();
        playerEquipmentRegistry.Initialize();
        ingredientRegistry.Initialize();
        recipeRegistry.Initialize(); // RecipeRegistry must be initialized after playerEquipmentRegistry and ingredientsRegistry
        fishRegistry.Initialize();
        monsterRegistry.Initialize();
        transitionRegistry.Initialize();
        eventRegistry.Initialize();

        // Initialize attributes
        currentTimeOfDay = TimeOfDayRegistry.daySO;
        currentMap = MapRegistry.driftwoodRiverSO; // Default map, for the first day of fishing
        daysCount = 1;
        nightsCount = 0;
        isFirstDay = true;
        isFirstNight = false;
        isFishingTutorialEnabled = true;
        isMapSelectionExplanationEnabled = true;
        isRecipeBookUnlocked = false;
        fishingPlayerPosition = defaultFishingPlayerPosition;
        fishingPlayerOrientation = defaultFishingPlayerOrientation;
    }


    // NAVIGATION FONCTIONS
    
    // Called by map selection to entering menu
    public void EnterMenu()
    {
        ChangeView(GameView.Main);
    }

    // Called by the menu manager to start a new game
    public void StartNewGame()
    {
        SaveSystem.DeleteSave();
        Start();

        currentEvent = EventRegistry.introductionSO;
        ChangeView(GameView.EventView);
    }

    // Called by the menu manager to continue game
    public void ContinueGame()
    {
        LoadGame();
        ChangeView(GameView.MapSelectionView);
    }

    // Called by the menu manager to quit game
    public void QuitGame()
    {
        SaveGame();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Called at the end of an event
    public void ExitEvent()
    {
        if (currentEvent == eventRegistry.introductionSO)
        {
            StartTimer();
            ChangeView(GameView.FishingView);
        }
        else if (currentEvent == eventRegistry.recipeBookObtentionSO)
        {
            isRecipeBookUnlocked = true;
            currentTransition = transitionRegistry.endRecipeBookEventSO;
            ChangeView(GameView.TransitionView); // Next view is in the ExitTransition function
            SaveAndChangeCurrentTimeOfDay();
        }
        else if (currentEvent == eventRegistry.endSO)
        {
            ChangeView(GameView.EndScreenView);
        }
    }

    // Called at the end of a transition, go to the next view
    public void ExitTransition()
    {
        ChangeView(GameView.MapSelectionView);
    }

    // Called in the FishingView at the end of the tutorial, unblock the timer
    public void EndOfFishingTutorial()
    {
        isFishingTutorialEnabled = false;
    }

    // Called in the MapSelection at the end of the explanations
    public void EndOfMapSelectionExplanation()
    {
        isMapSelectionExplanationEnabled = false;
    }

    // Called in the MapSelection Scene when clicking on a map
    public void SelectMap(MapSO mapSelected)
    {
        currentMap = mapSelected;
        StartTimer();
        ChangeView(GameView.FishingView);
    }

    // Called when clicking on the inventory button
    public void EnterInventory()
    {
        ChangeView(GameView.InventoryView);
    }

    // Called in the Inventory Scene when clicking Exit
    public void ExitInventory()
    {
        if (lastView == GameView.MapSelectionView)
        {
            ChangeView(GameView.MapSelectionView);   
        }
        else if (lastView == GameView.FishingView)
        {
            ChangeView(GameView.FishingView);
        }
    }

    // Called in the FishingView Scene in need of passing in monster view
    public void EnterMonsterView()
    {
        ChangeView(GameView.MonsterView);
    }

    // Called in the MonsterView Scene after winning
    public void WinAgainstMonster()
    {
        if (isFirstNight)
        {
            currentEvent = EventRegistry.recipeBookObtentionSO;
            ChangeView(GameView.EventView);
        }
        else
        {
            ChangeView(GameView.FishingView);
        }
    }

    // Called in the MonsterView Scene after dying
    public void DeathAgainstMonster()
    {
        // The player loses what he obtained last day and that night
        foreach (var ingredient in ingredientRegistry.AllIngredients)
        {
            RemoveIngredient(ingredient, obtainedIngredientLastDayAndNight[ingredient]);
        }

        currentTransition = transitionRegistry.deathAgainstMonsterSO;
        ChangeView(GameView.TransitionView); // Next view is in the ExitTransition function

        // The next day start
        SaveAndChangeCurrentTimeOfDay();
    }

    // Called when the time is out, we exit the fishing/monster view and return to the map selection
    private void TimeOut()
    {
        if (currentTimeOfDay == timeOfDayRegistry.daySO)
        {
            currentTransition = transitionRegistry.endDaySO;
            ChangeView(GameView.TransitionView); // Next view is in the ExitTransition function
        }
        else
        {            
            currentTransition = transitionRegistry.endNightSO;
            ChangeView(GameView.TransitionView); // Next view is in the ExitTransition function
        }

        // Change the time of day
        SaveAndChangeCurrentTimeOfDay();
    }

    // Called in the InventoryView when the player make the final remedy
    public void EnterEndEvent()
    {
        currentEvent = eventRegistry.endSO;
        ChangeView(GameView.EventView);
    }


    // INVENTORY, EQUIPMENT & PLAYER FUNCTIONS

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

    // Called when modifying fishing player position
    public void UpdateFishingPlayerPosition(Vector2 position)
    {
        fishingPlayerPosition = position;
    }

    // Called when modifying fishing player orientation
    public void UpdateFishingPlayerOrientation(Vector2 orientation)
    {
        fishingPlayerOrientation = orientation;
    }


    // HELPING FONCTIONS

    // Update the game (called at each frame of the game)
    private void Update()
    {
        // Handle the game update logic for views
        switch (currentView)
        {
            case GameView.Main:
                break;
                
            case GameView.EventView:
                break;
            
            case GameView.TransitionView:
                break;

            case GameView.MapSelectionView:
                break;

            case GameView.InventoryView:
                break;

            case GameView.FishingView:
                if (!IsFishingTutorialEnabled) { UpdateTimerAndCheckIfOut(); } // No timer during fishing tutorial
                break;

            case GameView.MonsterView:
                if (!IsFirstNight) { UpdateTimerAndCheckIfOut(); } // No timer during monster tutorial
                break;
            
            case GameView.EndScreenView:
                break;
        }
    }

    // Pass from one view to another
    private void ChangeView(GameView newView)
    {
        lastView = currentView;
        currentView = newView;

        switch (currentView)
        {
            case GameView.Main:
                SceneManager.LoadScene("Main");
                AudioManager.Instance.PlayMenuAndEventMusic();
                break;
            case GameView.EventView:
                SceneManager.LoadScene("EventView");
                AudioManager.Instance.PlayMenuAndEventMusic();
                break;
            case GameView.TransitionView:
                SceneManager.LoadScene("TransitionView");
                AudioManager.Instance.StopMusic();
                break;
            case GameView.MapSelectionView:
                SceneManager.LoadScene("MapSelectionView");
                AudioManager.Instance.PlayMapMusic();
                break;
            case GameView.InventoryView:
                SceneManager.LoadScene("InventoryView");
                // The inventory just keep playing the music of the scene from which it has been loaded
                break;
            case GameView.FishingView:
                SceneManager.LoadScene("FishingView");
                if ( currentTimeOfDay == TimeOfDayRegistry.daySO ) { AudioManager.Instance.PlayFishingDayMusic(); }
                else { AudioManager.Instance.PlayFishingNightMusic(); }
                break;
            case GameView.MonsterView:
                SceneManager.LoadScene("MonsterView");
                AudioManager.Instance.SaveFishingNightMusicTime();
                AudioManager.Instance.PlayMonsterMusic();
                break;
            case GameView.EndScreenView:
                SceneManager.LoadScene("EndScreenView");
                AudioManager.Instance.PlayMenuAndEventMusic();
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

    private void SaveAndChangeCurrentTimeOfDay()
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
            AudioManager.Instance.InitializeFishingNightMusicTime();
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

        // Player comes back to his default position & orientation
        fishingPlayerPosition = defaultFishingPlayerPosition;
        fishingPlayerOrientation = defaultFishingPlayerOrientation;

        SaveGame();
    }

    private void InitializeObtainedIngredientsLastDayAndNight()
    {
        obtainedIngredientLastDayAndNight = new Dictionary<IngredientSO, int>();
        foreach (var ingredient in ingredientRegistry.AllIngredients)
        {
            obtainedIngredientLastDayAndNight[ingredient] = 0;
        }
    }


    // DATA FUNCTIONS

    private void SaveGame()
    {
        SaveData data = new SaveData();

        data.currentTimeOfDayName = currentTimeOfDay.timeOfDayName;
        data.daysCount = daysCount;
        data.nightsCount = nightsCount;
        data.isFirstDay = isFirstDay;
        data.isFirstNight = isFirstNight;
        data.isFishingTutorialEnabled = isFishingTutorialEnabled;
        data.isMapSelectionExplanationEnabled = isMapSelectionExplanationEnabled;
        data.isRecipeBookUnlocked = isRecipeBookUnlocked;

        data.ingredients = ingredientRegistry.AllIngredients
            .Select(i => new IngredientSaveData
            {                
                ingredientName = i.ingredientName,
                playerQuantityPossessed = i.playerQuantityPossessed
            })
            .ToList();

        data.playerEquipments = playerEquipmentRegistry.AllPlayerEquipments
            .Select(e => new PlayerEquipmentSaveData
            {
                playerEquipmentName = e.playerEquipmentName,
                level = e.level
            })
            .ToList();
        
        data.recipes = recipeRegistry.AllRecipes
            .Select(r => new RecipeSaveData
            {
                recipeName = r.recipeName,
                hasAlreadyBeenUsed = r.hasAlreadyBeenUsed
            })
            .ToList();

        SaveSystem.Save(data);
    }

    private void LoadGame()
    {
        SaveData data = SaveSystem.Load();
        if (data == null) return;

        currentTimeOfDay = timeOfDayRegistry.GetByName(data.currentTimeOfDayName);
        daysCount = data.daysCount;
        nightsCount = data.nightsCount;
        isFirstDay = data.isFirstDay;
        isFirstNight = data.isFirstNight;
        isFishingTutorialEnabled = data.isFishingTutorialEnabled;
        isMapSelectionExplanationEnabled = data.isMapSelectionExplanationEnabled;
        isRecipeBookUnlocked = data.isRecipeBookUnlocked;

        foreach (var ingredientData in data.ingredients)
        {
            IngredientSO ingredient = ingredientRegistry.GetByName(ingredientData.ingredientName);
            ingredient.playerQuantityPossessed = ingredientData.playerQuantityPossessed;
        }

        foreach (var playerEquipmentData in data.playerEquipments)
        {
            PlayerEquipmentSO playerEquipment = playerEquipmentRegistry.GetByName(playerEquipmentData.playerEquipmentName);
            if (playerEquipmentData.level == 2 || playerEquipmentData.level == 3) {
                playerEquipment.UpgradeTo(2);
            }
            if (playerEquipmentData.level == 3) {
                playerEquipment.UpgradeTo(3);
            }
        }

        foreach (var recipeData in data.recipes)
        {
            RecipeSO recipe = recipeRegistry.GetByName(recipeData.recipeName);
            recipe.hasAlreadyBeenUsed = recipeData.hasAlreadyBeenUsed;
        }
    }
}
