using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryViewUIManager : MonoBehaviour
{
    // Allow to call InventoryViewUIManager.Instance anywhere (singleton)
    public static InventoryViewUIManager Instance { get; private set; }

    // Global UI elements
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject recipeBookPanel;

    [SerializeField] private TextMeshProUGUI pageText;

    [SerializeField] private GameObject recipeBookUnavailableImage;
    [SerializeField] private GameObject seeRecipeBookButton;

    [SerializeField] private TextMeshProUGUI fishingRodLevelText;
    [SerializeField] private TextMeshProUGUI boatLevelText;
    [SerializeField] private TextMeshProUGUI flashinglightLevelText;

    [SerializeField] private TextMeshProUGUI fishingRodDetailsText;
    [SerializeField] private TextMeshProUGUI boatDetailsText;
    [SerializeField] private TextMeshProUGUI flashinglightDetailsText;

    [SerializeField] private Image[] ingredientImages;
    [SerializeField] private TextMeshProUGUI[] ingredientTexts;

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

    // Update the fishing rod UI
    public void UpdateFishingRodUI(int level)
    {
        fishingRodLevelText.text = "Level " + level;

        var details = GameManager.Instance.PlayerEquipmentRegistry.fishingRodSO.detailsPerLevel;
        if (level == 1)
        {
            fishingRodDetailsText.text = details[0];
        }
        else if (level == 2)
        {
            fishingRodDetailsText.text = details[1];
        }
        else if (level == 3)
        {
            fishingRodDetailsText.fontSize = 45f;
            fishingRodDetailsText.text = details[1] + "\n" + details[2];
        }
    }

    // Update the boat UI
    public void UpdateBoatUI(int level)
    {
        boatLevelText.text = "Level " + level;

        var details = GameManager.Instance.PlayerEquipmentRegistry.boatSO.detailsPerLevel;
        if (level == 1)
        {
            boatDetailsText.text = details[0];
        }
        else if (level == 2)
        {
            boatDetailsText.text = details[1];
        }
        else if (level == 3)
        {
            boatDetailsText.fontSize = 45f;
            boatDetailsText.text = details[1] + "\n" + details[2];
        }
    }

    // Update the flashlight UI
    public void UpdateFlashlightUI(int level)
    {
        flashinglightLevelText.text = "Level " + level;

        var details = GameManager.Instance.PlayerEquipmentRegistry.flashlightSO.detailsPerLevel;
        if (level == 1)
        {
            flashinglightDetailsText.text = details[0];
        }
        else if (level == 2)
        {
            flashinglightDetailsText.text = details[1];
        }
        else if (level == 3)
        {
            flashinglightDetailsText.fontSize = 45f;
            flashinglightDetailsText.text = details[1] + "\n" + details[2];
        }
    }

    // Update the ingredients UI
    public void UpdateIngredientUI(int index, IngredientSO ingredient)
    {
        ingredientTexts[index].text = "x " + ingredient.playerQuantityPossessed;
        ingredientImages[index].sprite = ingredient.sprite;
        ingredientImages[index].color = ingredient.color;
    }

    // Display the UI for the Inventory state
    public void ShowInventoryStateUI()
    {
        inventoryPanel.SetActive(true);
        pageText.gameObject.SetActive(true);
        pageText.text = "Inventory";
        if (GameManager.Instance.IsRecipeBookUnlocked)
        {
            seeRecipeBookButton.SetActive(true);
            recipeBookUnavailableImage.SetActive(false);
        }
        else
        {
            seeRecipeBookButton.SetActive(false);
            recipeBookUnavailableImage.SetActive(true);
        }
    }

    // Display the UI for the Recipe Book state
    public void ShowRecipeBookStateUI()
    {
        recipeBookPanel.SetActive(true);
        pageText.gameObject.SetActive(true);
        pageText.text = "Recipe Book";
    }

    // Hide the UI for the Inventory state
    public void HideInventoryStateUI()
    {
        inventoryPanel.SetActive(false);
        pageText.gameObject.SetActive(false);
    }

    // Hide the UI for the Recipe Book state
    public void HideRecipeBookStateUI()
    {
        recipeBookPanel.SetActive(false);
        pageText.gameObject.SetActive(false);
    }
}
