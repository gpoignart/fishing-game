using UnityEngine;

public class MapSelectionGameManager : MonoBehaviour
{
    // Allow to call MapSelectionGameManager.Instance anywhere (singleton)
    public static MapSelectionGameManager Instance { get; private set; }

    // Internal references
    private string[] explanationTexts =
    {
        "Here is the global map. At the start of each day and night, you choose where to fish.",
        "Each location changes with time. Day and night affect which fish can be found. Rarer fish tend to appear after dark.",
        "Now… choose where you want to fish tonight."
    };
    private int indexOfExplanation;


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
        MapSelectionUIManager.Instance.UpdateBackgroundImage();
        MapSelectionUIManager.Instance.UpdateDayAndNightCounterText();
        MapSelectionUIManager.Instance.HideExplanationPanel();
        for (int i = 0; i < GameManager.Instance.MapRegistry.AllMaps.Length; i++)
        {
            if (GameManager.Instance.CurrentTimeOfDay == GameManager.Instance.TimeOfDayRegistry.daySO)
            {
                MapSelectionUIManager.Instance.UpdateMapButton(i, GameManager.Instance.MapRegistry.AllMaps[i].mapName, GameManager.Instance.MapRegistry.AllMaps[i].dayLogoSprite);
            }
            else
            {
                MapSelectionUIManager.Instance.UpdateMapButton(i, GameManager.Instance.MapRegistry.AllMaps[i].mapName, GameManager.Instance.MapRegistry.AllMaps[i].nightLogoSprite);   
            }
        }

        // Gives explanations the first time we enter the map
        if (GameManager.Instance.IsMapSelectionExplanationEnabled)
        {
            indexOfExplanation = 0;
            MapSelectionUIManager.Instance.DisableMapSelectionButtons();
            MapSelectionUIManager.Instance.HideChooseAMapText();
            MapSelectionUIManager.Instance.ShowExplanationPanel();
            MapSelectionUIManager.Instance.UpdateExplanationText(explanationTexts[indexOfExplanation]);
        }
    }

    private void Update()
    {
        // Click on next button with return
        if (GameManager.Instance.IsMapSelectionExplanationEnabled && Input.GetKeyDown(KeyCode.Return))
        {
            OnExplanationNextButtonPressed();
        }
    }

    // Called when the player clicks map button 1
    public void OnMapButton1Pressed()
    {
        GameManager.Instance.SelectMap(GameManager.Instance.MapRegistry.AllMaps[0]);
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    // Called when the player clicks map button 2
    public void OnMapButton2Pressed()
    {
        GameManager.Instance.SelectMap(GameManager.Instance.MapRegistry.AllMaps[1]);
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    // Called when the player clicks map button 3
    public void OnMapButton3Pressed()
    {
        GameManager.Instance.SelectMap(GameManager.Instance.MapRegistry.AllMaps[2]);
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    // Called when the player clicks menu button    
    public void OnMenuButtonPressed()
    {
        GameManager.Instance.EnterMenu();
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    // Called when the player clicks inventory button
    public void OnInventoryButtonPressed()
    {
        GameManager.Instance.EnterInventory();
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    // Called when the player clicks explanation next button
    public void OnExplanationNextButtonPressed()
    {
        indexOfExplanation ++;
        MapSelectionUIManager.Instance.UpdateExplanationText(explanationTexts[indexOfExplanation]);

        // If last explanation, all others buttons availables, and we hide the next button
        if (indexOfExplanation == explanationTexts.Length - 1)
        {
            MapSelectionUIManager.Instance.AbleMapSelectionButtons();
            MapSelectionUIManager.Instance.HideExplanationNextButton();
            GameManager.Instance.EndOfMapSelectionExplanation();
        }
        AudioManager.Instance.PlayPressingButtonSFX();
    }
}
