using UnityEngine;

public class MapSelectionGameManager : MonoBehaviour
{
    // Allow to call MapSelectionGameManager.Instance anywhere (singleton)
    public static MapSelectionGameManager Instance { get; private set; }

    // Internal references
    private string[] explanationTexts =
    {
        "Here's the global map. At the beginning of each day and each night, you must choose a place to fish.",
        "In each location, and depending on whether it is day or night, the types of fish accessible differ. Rare fish are more likely to spawn at night.",
        "Speaking of which, click on the place where you want to fish tonight!"
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
        MapSelectionUIManager.Instance.HideExplanationPanel();
        for (int i = 0; i < GameManager.Instance.MapRegistry.AllMaps.Length; i++)
        {
            MapSelectionUIManager.Instance.UpdateMapButtonText(i, GameManager.Instance.MapRegistry.AllMaps[i].mapName);
        }
        MapSelectionUIManager.Instance.UpdateDayAndNightCounterText();

        // Gives explanations the first time we enter the map
        if (GameManager.Instance.IsMapSelectionExplanationEnabled)
        {
            indexOfExplanation = 0;
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
        if (GameManager.Instance.IsMapSelectionExplanationEnabled) { return; }
        GameManager.Instance.SelectMap(GameManager.Instance.MapRegistry.AllMaps[0]);
    }

    // Called when the player clicks map button 2
    public void OnMapButton2Pressed()
    {
        if (GameManager.Instance.IsMapSelectionExplanationEnabled) { return; }
        GameManager.Instance.SelectMap(GameManager.Instance.MapRegistry.AllMaps[1]);
    }

    // Called when the player clicks map button 3
    public void OnMapButton3Pressed()
    {
        if (GameManager.Instance.IsMapSelectionExplanationEnabled) { return; }
        GameManager.Instance.SelectMap(GameManager.Instance.MapRegistry.AllMaps[2]);
    }

    // Called when the player clicks inventory button
    public void OnInventoryButtonPressed()
    {
        if (GameManager.Instance.IsMapSelectionExplanationEnabled) { return; }
        GameManager.Instance.EnterInventory();
    }

    // Called when the player clicks explanation next button
    public void OnExplanationNextButtonPressed()
    {
        indexOfExplanation ++;
        MapSelectionUIManager.Instance.UpdateExplanationText(explanationTexts[indexOfExplanation]);

        // If last explanation, all others buttons availables, and we hide the next button
        if (indexOfExplanation == explanationTexts.Length - 1)
        {
            MapSelectionUIManager.Instance.HideExplanationNextButton();
            GameManager.Instance.EndOfMapSelectionExplanation();
        }
    }
}
