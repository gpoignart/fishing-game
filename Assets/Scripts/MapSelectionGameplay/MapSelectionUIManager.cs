using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapSelectionUIManager : MonoBehaviour
{
    // Allow to call MapSelectionUIManager.Instance anywhere (singleton)
    public static MapSelectionUIManager Instance { get; private set; }

    [SerializeField] private Sprite dayBackgroundSprite;
    [SerializeField] private Color dayBackgroundColor; // TO REMOVE WHEN REAL SPRITE
    [SerializeField] private Sprite nightBackgroundSprite;
    [SerializeField] private Color nightBackgroundColor;  // TO REMOVE WHEN REAL SPRITE
    [SerializeField] private Image backgroundImage;
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
            backgroundImage.color = dayBackgroundColor; // TO REMOVE WHEN REAL SPRITE
        }
        else
        {
            backgroundImage.sprite = nightBackgroundSprite;
            backgroundImage.color = nightBackgroundColor; // TO REMOVE WHEN REAL SPRITE            
        }
    }

    public void UpdateMapButtonText(int index, string mapName)
    {
        mapButtonTexts[index].text = mapName;
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
