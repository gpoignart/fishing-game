using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeBookUIManager : MonoBehaviour
{
    // Allow to call RecipeBookUIManager.Instance anywhere (singleton)
    public static RecipeBookUIManager Instance { get; private set; }

    // Global UI elements
    [SerializeField] private TextMeshProUGUI recipeNameTextL;
    [SerializeField] private TextMeshProUGUI recipeDescriptionTextL;
    [SerializeField] private Transform ingredientPanelL;
    [SerializeField] private Button makeRecipeL;
    [SerializeField] private TextMeshProUGUI instructionTextL;

    [SerializeField] private TextMeshProUGUI recipeNameTextR;
    [SerializeField] private TextMeshProUGUI recipeDescriptionTextR;
    [SerializeField] private Transform ingredientPanelR;
    [SerializeField] private Button makeRecipeR;
    [SerializeField] private TextMeshProUGUI instructionTextR;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

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

    public void ShowPreviousButton()
    {
        previousButton.gameObject.SetActive(true);
    }

    public void HidePreviousButton()
    {
        previousButton.gameObject.SetActive(false);
    }

    public void ShowNextButton()
    {
        nextButton.gameObject.SetActive(true);
    }

    public void HideNextButton()
    {
        nextButton.gameObject.SetActive(false);
    }

    public void UpdateLeftPageUI(RecipeSO recipe, int currentLeftRecipeAvailableCode)
    {
        if (recipe == null)
        {
            EmptyOnePageUI(recipeNameTextL, recipeDescriptionTextL, ingredientPanelL, makeRecipeL, instructionTextL);
        }
        else
        {
            UpdateOnePageUI(recipe, recipeNameTextL, recipeDescriptionTextL, ingredientPanelL, makeRecipeL, instructionTextL, currentLeftRecipeAvailableCode);
        }
    }

    public void UpdateRightPageUI(RecipeSO recipe, int currentRightRecipeAvailableCode)
    {
        if (recipe == null)
        {
            EmptyOnePageUI(recipeNameTextR, recipeDescriptionTextR, ingredientPanelR, makeRecipeR, instructionTextR);
        }
        else
        {
            UpdateOnePageUI(recipe, recipeNameTextR, recipeDescriptionTextR, ingredientPanelR, makeRecipeR, instructionTextR, currentRightRecipeAvailableCode);
        }
    }

    public void EmptyOnePageUI(TextMeshProUGUI recipeNameText, TextMeshProUGUI recipeDescriptionText, Transform ingredientPanel, Button makeRecipeButton, TextMeshProUGUI instructionText)
    {
        recipeNameText.text = "";
        recipeDescriptionText.text = "";
        makeRecipeButton.gameObject.SetActive(false);
        instructionText.text = "";

        // Empty previous elements
        foreach (Transform child in ingredientPanel) Destroy(child.gameObject);
    }

    public void UpdateOnePageUI(RecipeSO recipe, TextMeshProUGUI recipeNameText, TextMeshProUGUI recipeDescriptionText, Transform ingredientPanel, Button makeRecipeButton, TextMeshProUGUI instructionText, int currentRecipeAvailableCode)
    {
        recipeNameText.text = recipe.recipeName;
        recipeDescriptionText.text = recipe.description;
        makeRecipeButton.gameObject.SetActive(true);

        // Empty previous elements
        foreach (Transform child in ingredientPanel) Destroy(child.gameObject);

        // Create new ingredients with prefab
        foreach (RecipeIngredient recipeIngredient in recipe.ingredients)
        {
            // Create a new ingredient line
            GameObject newRecipeIngredientLine = Instantiate(RecipeBookGameManager.Instance.RecipeIngredientLineUIPrefab, ingredientPanel);

            // Change the text and the image
            Image img = newRecipeIngredientLine.transform.Find("RecipeIngredientImage").GetComponent<Image>();
            TextMeshProUGUI text = newRecipeIngredientLine.transform.Find("RecipeIngredientText").GetComponent<TextMeshProUGUI>();

            img.sprite = recipeIngredient.ingredientSO.sprite;
            text.text = $"{recipeIngredient.ingredientSO.playerQuantityPossessed}/{recipeIngredient.quantity} - {recipeIngredient.ingredientSO.ingredientName}";
        }

        // Handle make recipe button and instruction text
        if (currentRecipeAvailableCode == 0)
        {
            makeRecipeButton.interactable = true;
            instructionText.text = "";
        }
        else if (currentRecipeAvailableCode == 1)
        {
            makeRecipeButton.interactable = false;
            instructionText.text = "Not enough ingredients.";
        }
        else if (currentRecipeAvailableCode == 2)
        {
            makeRecipeButton.interactable = false;
            instructionText.text = "Equipment needs to be level 2 before.";
        }
        else if (currentRecipeAvailableCode == 3)
        {
            makeRecipeButton.interactable = false;
            instructionText.text = "Recipe has already been used.";
        }
    }
}

