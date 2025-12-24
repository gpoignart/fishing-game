using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MapSelectionUIManager : MonoBehaviour
{
    // Allow to call MapSelectionUIManager.Instance anywhere (singleton)
    public static MapSelectionUIManager Instance { get; private set; }

    // Sprites
    [SerializeField] private Sprite dayBackgroundSprite;
    [SerializeField] private Sprite nightBackgroundSprite;

    // UI elements
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image[] mapButtonImages;
    [SerializeField] private Transform[] mapIngredientBubbleContainers;
    [SerializeField] private TextMeshProUGUI[] mapButtonTexts;
    [SerializeField] private TextMeshProUGUI dayAndNightCounterText;
    [SerializeField] private TextMeshProUGUI chooseAMapText;
    [SerializeField] private GameObject explanationPanel;
    [SerializeField] private TextMeshProUGUI explanationText;
    [SerializeField] private GameObject explanationNextButton;
    [SerializeField] private CanvasGroup mapSelectionButtons;
    
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

    public void UpdateBackgroundImage()
    {
        if (GameManager.Instance.CurrentTimeOfDay == GameManager.Instance.TimeOfDayRegistry.daySO)
        {
            backgroundImage.sprite = dayBackgroundSprite;
        }
        else
        {
            backgroundImage.sprite = nightBackgroundSprite;         
        }
    }

    public void UpdateDayAndNightCounterText()
    {
        if (GameManager.Instance.CurrentTimeOfDay == GameManager.Instance.TimeOfDayRegistry.daySO)
        {
            dayAndNightCounterText.text = $"DAY {GameManager.Instance.DaysCount}";
        }
        else
        {
            dayAndNightCounterText.text = $"NIGHT {GameManager.Instance.NightsCount}";
        }
    }

    public void UpdateMapButton(int index, string mapName, Sprite mapLogo)
    {
        mapButtonTexts[index].text = mapName;
        mapButtonImages[index].sprite = mapLogo;
    }

    public void UpdateIngredientBubbles(int mapIndex, List<IngredientSO> ingredients)
    {
        Transform bubbleContainer = mapIngredientBubbleContainers[mapIndex];

        int bubbleCount = bubbleContainer.childCount;

        for (int i = 0; i < bubbleCount; i++)
        {
            Transform bubble = bubbleContainer.GetChild(i);

            // Set bubble color
            if (GameManager.Instance.CurrentTimeOfDay == GameManager.Instance.TimeOfDayRegistry.daySO)
            {
                bubble.GetComponent<Image>().color = GameManager.Instance.MapRegistry.AllMaps[mapIndex].dayBubbleColor;
            }
            else
            {
                bubble.GetComponent<Image>().color = GameManager.Instance.MapRegistry.AllMaps[mapIndex].nightBubbleColor;
            }

            // Set ingredients images
            Image ingredientImage = bubble.GetChild(0).GetComponent<Image>();

            if (i < ingredients.Count)
            {
                ingredientImage.sprite = ingredients[i].sprite;
                bubble.gameObject.SetActive(true);
            }
            else
            {
                // Not enough ingredient, we desactive the bubble
                bubble.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateExplanationText(string explanationTextContent)
    {
        explanationText.text = explanationTextContent;
    }

    public void ShowExplanationPanel()
    {
        explanationPanel.SetActive(true);
    }

    public void HideExplanationPanel()
    {
        explanationPanel.SetActive(false);   
    }
    
    public void HideChooseAMapText()
    {
        chooseAMapText.gameObject.SetActive(false);
    }

    public void HideExplanationNextButton()
    {
        explanationNextButton.SetActive(false);
    }

    public void DisableMapSelectionButtons()
    {
        mapSelectionButtons.interactable = false;
    }

    public void AbleMapSelectionButtons()
    {
        mapSelectionButtons.interactable = true;
    }
}
