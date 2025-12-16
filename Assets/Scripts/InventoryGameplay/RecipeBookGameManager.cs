using UnityEngine;

public class RecipeBookGameManager : MonoBehaviour
{
    // Allow to call RecipeBookGameManager.Instance anywhere (singleton)
    public static RecipeBookGameManager Instance { get; private set; }

    // Attributes
    [SerializeField] private GameObject recipeIngredientLineUIPrefab;

    // READ-ONLY ATTRIBUTES
    public GameObject RecipeIngredientLineUIPrefab => recipeIngredientLineUIPrefab;

    // Internal attributes
    private int currentPageIndex = 0; // Index of the actual left page
    private RecipeSO currentLeftRecipe;
    private int currentLeftRecipeAvailableCode;
    private RecipeSO currentRightRecipe;
    private int currentRightRecipeAvailableCode;

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

    private void Start()
    {
        UpdateCurrentRecipes();
        UpdateUI();
    }

    public void OnMakeRecipeLeftButtonPressed()
    {
        MakeRecipe(currentLeftRecipe);
        UpdateCurrentRecipes();
        UpdateUI();
    }

    public void OnMakeRecipeRightButtonPressed()
    {
        MakeRecipe(currentRightRecipe);
        UpdateCurrentRecipes();
        UpdateUI();
    }

    public void OnPreviousButtonPressed()
    {
        currentPageIndex -= 2;
        UpdateCurrentRecipes();
        UpdateUI();
    }

    public void OnNextButtonPressed()
    {
        currentPageIndex += 2;
        UpdateCurrentRecipes();
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Previous button
        if (currentPageIndex > 0) { RecipeBookUIManager.Instance.ShowPreviousButton(); }
        else { RecipeBookUIManager.Instance.HidePreviousButton(); }

        // Next button
        if (currentPageIndex + 2 < GameManager.Instance.RecipeRegistry.AllRecipes.Length) { RecipeBookUIManager.Instance.ShowNextButton(); }
        else { RecipeBookUIManager.Instance.HideNextButton(); }

        // Recipe pages
        RecipeBookUIManager.Instance.UpdateLeftPageUI(currentLeftRecipe, currentLeftRecipeAvailableCode);
        RecipeBookUIManager.Instance.UpdateRightPageUI(currentRightRecipe, currentRightRecipeAvailableCode);
    }

    // Find current recipes
    private void UpdateCurrentRecipes()
    {
        currentLeftRecipe = null;
        currentRightRecipe = null;

        if (GameManager.Instance.RecipeRegistry.AllRecipes.Length > currentPageIndex)
        {
            currentLeftRecipe = GameManager.Instance.RecipeRegistry.AllRecipes[currentPageIndex];
            currentLeftRecipeAvailableCode = checkRecipeAvailable(currentLeftRecipe);
        }

        if (GameManager.Instance.RecipeRegistry.AllRecipes.Length > currentPageIndex + 1)
        {
            currentRightRecipe = GameManager.Instance.RecipeRegistry.AllRecipes[currentPageIndex + 1];
            currentRightRecipeAvailableCode = checkRecipeAvailable(currentRightRecipe);
        }
    }

    // Return 0 if available, 1 if equipment is level 1 and recipe is level 3, 2 if not enough ingredients, 3 if the recipe has already been used
    private int checkRecipeAvailable(RecipeSO recipe)
    {
        // Recipe already used
        if (recipe.hasAlreadyBeenUsed) { return 3; }
        
        // Check if enough ingredients
        foreach (var recipeIngredient in recipe.ingredients)
        {
            if (recipeIngredient.ingredientSO.playerQuantityPossessed < recipeIngredient.quantity)
            {
                return 2;
            }
        }

        // Recipe for a 3 upgrade if a 2 upgrade has not been made (the final recipe is an exception)
        if (!recipe.isFinalRecipe && recipe.upgradesEquipment.level != 2 && recipe.upgradesToLevel == 3) { return 1; }

        return 0;
    }

    // Update inventory according to the recipe
    private void MakeRecipe(RecipeSO recipe)
    {
        if (recipe.isFinalRecipe)
        {
            GameManager.Instance.EnterEndEvent();
        }
        else
        {
            // Remove player ingredients
            foreach (var recipeIngredient in recipe.ingredients)
            {
                GameManager.Instance.RemoveIngredient(recipeIngredient.ingredientSO, recipeIngredient.quantity);
            }

            // Upgrade equipment
            GameManager.Instance.UpgradeEquipment(recipe.upgradesEquipment, recipe.upgradesToLevel);

            // Recipe used
            recipe.hasAlreadyBeenUsed = true;
        }
    }
}
