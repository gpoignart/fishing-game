using UnityEngine;
using TMPro;

public class MapSelectionUIManager : MonoBehaviour
{
    // Allow to call MapSelectionUIManager.Instance anywhere (singleton)
    public static MapSelectionUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI[] mapButtonTexts;
    [SerializeField] private TextMeshProUGUI dayAndNightCounterText;
    [SerializeField] private TextMeshProUGUI chooseAMapText;
    [SerializeField] private GameObject explanationPanel;
    [SerializeField] private TextMeshProUGUI explanationText;
    [SerializeField] private GameObject explanationNextButton;
    
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

    public void UpdateMapButtonText(int index, string mapName)
    {
        mapButtonTexts[index].text = mapName;
    }

    public void UpdateDayAndNightCounterText()
    {
        if (GameManager.Instance.CurrentTimeOfDay == GameManager.Instance.TimeOfDayRegistry.daySO)
        {
            dayAndNightCounterText.text = $"Day {GameManager.Instance.DaysCount}";
        }
        else
        {
            dayAndNightCounterText.text = $"Night {GameManager.Instance.NightsCount}";
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
}
