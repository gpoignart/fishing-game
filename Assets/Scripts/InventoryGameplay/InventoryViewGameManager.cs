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

        // First state
        ChangeState(InventoryViewGameState.Inventory);
    }

    // Exit inventory
    public void OnExitButtonPressed()
    {
        GameManager.Instance.ExitInventory();
    }

    // See Inventory
    public void OnSeeInventoryButtonPressed()
    { 
        ChangeState(InventoryViewGameState.Inventory);
    }

    // See Recipe Book
    public void OnSeeRecipeBookButtonPressed()
    { 
        ChangeState(InventoryViewGameState.RecipeBook);
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
