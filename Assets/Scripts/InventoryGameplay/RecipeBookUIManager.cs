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

    [SerializeField] private TextMeshProUGUI recipeNameTextR;
    [SerializeField] private TextMeshProUGUI recipeDescriptionTextR;
    [SerializeField] private Transform ingredientPanelR;
    [SerializeField] private Button makeRecipeR;

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

    public void MakeRecipeLeftButtonInteractive()
    {
        makeRecipeL.interactable = true;
    }

    public void MakeRecipeLeftButtonNotInteractive()
    {
        makeRecipeL.interactable = false;
    }

    public void MakeRecipeRightButtonInteractive()
    {
        makeRecipeR.interactable = true;
    }

    public void MakeRecipeRightButtonNotInteractive()
    {
        makeRecipeR.interactable = false;
    }

    public void UpdateLeftPageUI(RecipeSO recipe)
    {
        if (recipe == null)
        {
            EmptyOnePageUI(recipeNameTextL, recipeDescriptionTextL, ingredientPanelL, makeRecipeL);
        }
        else
        {
            UpdateOnePageUI(recipe, recipeNameTextL, recipeDescriptionTextL, ingredientPanelL, makeRecipeL);
        }
    }

    public void UpdateRightPageUI(RecipeSO recipe)
    {
        if (recipe == null)
        {
            EmptyOnePageUI(recipeNameTextR, recipeDescriptionTextR, ingredientPanelR, makeRecipeR);
        }
        else
        {
            UpdateOnePageUI(recipe, recipeNameTextR, recipeDescriptionTextR, ingredientPanelR, makeRecipeR);
        }
    }

    public void EmptyOnePageUI(TextMeshProUGUI recipeNameText, TextMeshProUGUI recipeDescriptionText, Transform ingredientPanel, Button makeRecipeButton)
    {
        recipeNameText.text = "";
        recipeDescriptionText.text = "";
        makeRecipeButton.gameObject.SetActive(false);

        // Empty previous elements
        foreach (Transform child in ingredientPanel) Destroy(child.gameObject);
    }

    public void UpdateOnePageUI(RecipeSO recipe, TextMeshProUGUI recipeNameText, TextMeshProUGUI recipeDescriptionText, Transform ingredientPanel, Button makeRecipeButton)
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
            img.color = recipeIngredient.ingredientSO.color;
            text.text = $"{recipeIngredient.quantity} x {recipeIngredient.ingredientSO.ingredientName}";
        }
    }
}

