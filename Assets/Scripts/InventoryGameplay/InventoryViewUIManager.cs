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
    [SerializeField] private GameObject recipeBookAttentionMarks;

    [SerializeField] private TextMeshProUGUI fishingRodLevelText;
    [SerializeField] private TextMeshProUGUI boatLevelText;
    [SerializeField] private TextMeshProUGUI flashinglightLevelText;

    [SerializeField] private TextMeshProUGUI fishingRodDetailsText;
    [SerializeField] private TextMeshProUGUI boatDetailsText;
    [SerializeField] private TextMeshProUGUI flashinglightDetailsText;

    [SerializeField] private Image[] ingredientBoxes;
    [SerializeField] private Image[] ingredientImages;
    [SerializeField] private TextMeshProUGUI[] ingredientTexts;

    [SerializeField] private RectTransform ingredientInfoPanelRect;
    [SerializeField] private TextMeshProUGUI ingredientPanelIngredientName;
    [SerializeField] private Image ingredientPanelIngredientImage;
    [SerializeField] private Image ingredientPanelFishImage;
    [SerializeField] private Transform ingredientPanelMapsContainer;
    [SerializeField] private GameObject ingredientPanelMapIconPrefab;

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

    // Show attention mark around the recipe book logo
    public void ShowAttentionMarkRecipeBook()
    {
        recipeBookAttentionMarks.SetActive(true);
    }

    // Hide attention mark around the recipe book logo
    public void HideAttentionMarkRecipeBook()
    {
        recipeBookAttentionMarks.SetActive(false);
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

    // Update the ingredients UI and attribute the right ingredient to the hover
    public void UpdateIngredientUI(int index, IngredientSO ingredient)
    {
        ingredientTexts[index].text = "x " + ingredient.playerQuantityPossessed;
        ingredientImages[index].sprite = ingredient.sprite;

        IngredientHoverManager hover = ingredientBoxes[index].GetComponent<IngredientHoverManager>();
        hover.ingredient = ingredient;
    }


    // Show the ingredients panel UI
    public void ShowIngredientPanelUI(RectTransform hoveredBox, string ingredientName, Sprite ingredientSprite, Sprite fishSprite, Sprite[] mapSprites)
    {
        // To do first to read the size
        UpdateIngredientPanelUI(ingredientName, ingredientSprite, fishSprite, mapSprites);
        ingredientInfoPanelRect.gameObject.SetActive(true);

        // Place the panel ahead the hovered box without being out of the screen bounds
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(ingredientInfoPanelRect);

        RectTransform canvasRect = ingredientInfoPanelRect.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        Vector2 ingredientInfoPanelSize = ingredientInfoPanelRect.rect.size;
        Vector2 canvasSize = canvasRect.rect.size;

        Vector3[] corners = new Vector3[4];
        hoveredBox.GetWorldCorners(corners);
        Vector3 topCenterWorld = (corners[1] + corners[2]) * 0.5f;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            RectTransformUtility.WorldToScreenPoint(null, topCenterWorld),
            null,
            out localPoint
        );

        float clampedX = Mathf.Clamp(
            localPoint.x,
            -canvasSize.x / 2 + ingredientInfoPanelSize.x / 2,
            canvasSize.x / 2 - ingredientInfoPanelSize.x / 2
        );

        ingredientInfoPanelRect.localPosition = new Vector2(
            clampedX,
            ingredientInfoPanelRect.localPosition.y
        );
    }

    // Hide the ingredients panel UI
    public void HideIngredientPanelUI()
    {
        ingredientInfoPanelRect.gameObject.SetActive(false);
    }

    // Update the ingredients panel UI
    public void UpdateIngredientPanelUI(string ingredientName, Sprite ingredientSprite, Sprite fishSprite, Sprite[] mapSprites)
    {
        ingredientPanelIngredientName.text = ingredientName;
        ingredientPanelIngredientImage.sprite = ingredientSprite;
        ingredientPanelFishImage.sprite = fishSprite;
        ingredientPanelFishImage.SetNativeSize();

        // Delete previous map icons       
        foreach (Transform child in ingredientPanelMapsContainer) { Destroy(child.gameObject); }

        // Create map icons
        foreach (Sprite map in mapSprites)
        {
            GameObject icon = Instantiate(ingredientPanelMapIconPrefab, ingredientPanelMapsContainer);
            icon.GetComponent<Image>().sprite = map;
        }

        // Force layout rebuild
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            GetComponent<RectTransform>()
        );
    }


    // Display the UI for the Inventory state
    public void ShowInventoryStateUI()
    {
        inventoryPanel.SetActive(true);
        HideIngredientPanelUI();
        HideAttentionMarkRecipeBook();
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

        if (GameManager.Instance.DaysCount == 2 && !GameManager.Instance.IsRecipeBookOpened)
        {
            ShowAttentionMarkRecipeBook();
        }
    }

    // Display the UI for the Recipe Book state
    public void ShowRecipeBookStateUI()
    {
        recipeBookPanel.SetActive(true);
        pageText.gameObject.SetActive(true);
        pageText.text = "Recipe Book";
        RecipeBookGameManager.Instance.Start();
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
