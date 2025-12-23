using UnityEngine;

public class InventoryViewGameManager : MonoBehaviour
{
    // Allow to call InventoryViewGameManager.Instance anywhere (singleton)
    public static InventoryViewGameManager Instance { get; private set; }

    // Internal states
    private enum InventoryViewGameState
    {
        Inventory,
        RecipeBook
    }
    private InventoryViewGameState currentState;

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

    void Start()
    {
        InventoryViewUIManager.Instance.HideRecipeBookStateUI();
        InventoryViewUIManager.Instance.ShowInventoryStateUI();
        InventoryViewUIManager.Instance.HideIngredientPanelUI();

        // First state
        ChangeState(InventoryViewGameState.Inventory);
    }

    // Exit inventory
    public void OnExitButtonPressed()
    {
        GameManager.Instance.ExitInventory();
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    // See Inventory
    public void OnSeeInventoryButtonPressed()
    { 
        ChangeState(InventoryViewGameState.Inventory);
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    // See Recipe Book
    public void OnSeeRecipeBookButtonPressed()
    { 
        ChangeState(InventoryViewGameState.RecipeBook);
        AudioManager.Instance.PlayPressingButtonSFX();
    }
    
    // Show ingredient panel, called by the ingredientHoverUI
    public void OnIngredientHoverEnter(RectTransform hoveredBox, IngredientSO ingredient)
    {
        FishSO fish = GameManager.Instance.FishRegistry.GetFishFromIngredient(ingredient);

        // Determine fish spawn times
        bool day = false;
        bool night = false;
        foreach (TimeOfDaySO time in fish.spawnTimes)
        {
            if (time == GameManager.Instance.TimeOfDayRegistry.daySO) { day = true; }

            if (time == GameManager.Instance.TimeOfDayRegistry.nightSO) { night = true; }
        }

        // Count how many map icons we need
        int iconsPerMap = (day && night) ? 2 : 1;
        int totalIcons = fish.spawnMaps.Length * iconsPerMap;

        // Create the array
        Sprite[] mapSprites = new Sprite[totalIcons];
        int index = 0;
        foreach (MapSO map in fish.spawnMaps)
        {
            if (day) { mapSprites[index++] = map.dayLogoSprite; }
            if (night) { mapSprites[index++] = map.nightLogoSprite; }
        }

        InventoryViewUIManager.Instance.ShowIngredientPanelUI(hoveredBox, ingredient.ingredientName, ingredient.sprite, fish.sprite, mapSprites);
    }

    // Hide ingredient panel (hover exit)
    public void OnIngredientHoverExit()
    {
        InventoryViewUIManager.Instance.HideIngredientPanelUI();
    }

    // Pass from one state to another
    private void ChangeState(InventoryViewGameState newState)
    {
        // Exit logic for the previous state
        OnStateExit(currentState);

        currentState = newState;

        // Enter logic for the new state
        OnStateEnter(currentState);
    }

    // Handle the game logic when entering states
    private void OnStateEnter(InventoryViewGameState state)
    {
        switch (state)
        {
            case InventoryViewGameState.Inventory:
                Debug.Log("Entering Inventory state");
                UpdateInventoryUI();
                InventoryViewUIManager.Instance.ShowInventoryStateUI();
                break;

            case InventoryViewGameState.RecipeBook:
                Debug.Log("Entering Recipe Book state");
                InventoryViewUIManager.Instance.ShowRecipeBookStateUI();
                break;
        }
    }

    // Handle the game logic when exiting states
    private void OnStateExit(InventoryViewGameState state)
    {
        switch (state)
        {
            case InventoryViewGameState.Inventory:
                Debug.Log("Exiting Inventory state");
                InventoryViewUIManager.Instance.HideInventoryStateUI();
                break;

            case InventoryViewGameState.RecipeBook:
                Debug.Log("Exiting Recipe Book state");
                InventoryViewUIManager.Instance.HideRecipeBookStateUI();
                break;
        }
    }

    private void UpdateInventoryUI()
    {
        // Update the equipment UI
        InventoryViewUIManager.Instance.UpdateFishingRodUI(GameManager.Instance.PlayerEquipmentRegistry.fishingRodSO.level);
        InventoryViewUIManager.Instance.UpdateBoatUI(GameManager.Instance.PlayerEquipmentRegistry.boatSO.level);
        InventoryViewUIManager.Instance.UpdateFlashlightUI(GameManager.Instance.PlayerEquipmentRegistry.flashlightSO.level);

        // Update the ingredients UI
        for (int i = 0; i < GameManager.Instance.IngredientRegistry.AllIngredients.Length; i++)
        {
            IngredientSO ingredient = GameManager.Instance.IngredientRegistry.AllIngredients[i];
            int count = ingredient.playerQuantityPossessed;
            InventoryViewUIManager.Instance.UpdateIngredientUI(i, ingredient);
        }
    }
}
