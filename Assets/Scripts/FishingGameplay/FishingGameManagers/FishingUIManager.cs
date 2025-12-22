using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FishingUIManager : MonoBehaviour
{
    // Allows to call FishingUIManager.Instance anywhere (singleton)
    public static FishingUIManager Instance { get; private set; }

    // Global UI elements
    [SerializeField]
    private SpriteRenderer bottomBackgroundSpriteRenderer;
    
    [SerializeField]
    private SpriteRenderer topBackgroundSpriteRenderer;

    [SerializeField]
    private Button inventoryButton;

    [SerializeField]
    private GameObject timer;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private GameObject hookButton;

    [SerializeField]
    private GameObject dragBar;

    [SerializeField]
    private GameObject commandsPanel;

    [SerializeField]
    private GameObject extendCommandsButton;

    [SerializeField]
    private GameObject collapseCommandsButton;

    [SerializeField]
    private GameObject extendedCommands;

    [SerializeField]
    private GameObject loot;

    [SerializeField]
    private GameObject loseFishText;

    [SerializeField]
    private Image lootImage;

    [SerializeField]
    private GameObject tutorialPanel;

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

    // Initialize the UI (called when the game start)
    void Start()
    {
        if (!GameManager.Instance.IsFishingTutorialEnabled)
        {
            HideTutorialPanel();
        }
        InitializeCommandsPanel();
        hookButton.SetActive(false);
        dragBar.SetActive(false);
        loot.SetActive(false);
        loseFishText.SetActive(false);

        // Initialize backgrounds sprites
        if (GameManager.Instance.CurrentTimeOfDay == GameManager.Instance.TimeOfDayRegistry.daySO)
        {
            bottomBackgroundSpriteRenderer.sprite = GameManager.Instance.CurrentMap.dayBottomBackgroundSprite;
            topBackgroundSpriteRenderer.sprite = GameManager.Instance.CurrentMap.dayTopBackgroundSprite;
        }
        else
        {
            bottomBackgroundSpriteRenderer.sprite = GameManager.Instance.CurrentMap.nightBottomBackgroundSprite;
            topBackgroundSpriteRenderer.sprite = GameManager.Instance.CurrentMap.nightTopBackgroundSprite;
        }
    }

    // Display the UI for the hooking state
    public void ShowHookingStateUI()
    {
        hookButton.SetActive(true);
    }

    // Hide the UI for the hooking state
    public void HideHookingStateUI()
    {
        hookButton.SetActive(false);
    }

    // Display the UI for the fishing state
    public void ShowFishingStateUI()
    {
        dragBar.SetActive(true);
    }

    // Update the timer display
    public void UpdateTimerUI(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    // Show the loot for duration seconds
    public IEnumerator ShowLootForSeconds(IngredientSO ingredient, float duration, float fadeDuration)
    {
        loot.SetActive(true);

        lootImage.sprite = ingredient.sprite;

        yield return StartCoroutine(FadeIn(loot, fadeDuration));

        yield return new WaitForSeconds(duration);
        
        yield return StartCoroutine(FadeOut(loot, fadeDuration));

        loot.SetActive(false);
    }

    // Show the loot
    public void ShowLoot(IngredientSO ingredient)
    {
        loot.SetActive(true);
        lootImage.sprite = ingredient.sprite;
    }

    // Hide the loot
    public void HideLoot()
    {
        loot.SetActive(false);
    }

    // Show the loseFishText for duration seconds
    public IEnumerator ShowLoseFishTextForSeconds(float duration, float fadeDuration)
    {
        loseFishText.SetActive(true);

        yield return StartCoroutine(FadeIn(loseFishText, fadeDuration));

        yield return new WaitForSeconds(duration);
        
        yield return StartCoroutine(FadeOut(loseFishText, fadeDuration));

        loseFishText.SetActive(false);
    }

    // Hide the UI for the fishing state
    public void HideFishingStateUI()
    {
        dragBar.SetActive(false);
    }

    // Tutorial panel
    public void ShowTutorialPanel()
    {
        tutorialPanel.SetActive(true);
    }

    public void HideTutorialPanel()
    {
        tutorialPanel.SetActive(false);
    }

    // Inventory button
    public void ShowInventoryButton()
    {
        inventoryButton.gameObject.SetActive(true);
    }

    public void HideInventoryButton()
    {
        inventoryButton.gameObject.SetActive(false);
    }
    
    public void AbleInventoryButton()
    {
        inventoryButton.interactable = true;
    }

    public void DisableInventoryButton()
    {
        inventoryButton.interactable = false;
    }

    // Timer
    public void ShowTimer()
    {
        timer.SetActive(true);
    }

    public void HideTimer()
    {
        timer.SetActive(false);
    }

    // Commands Panel
    public void ShowCommandsPanel()
    {
        commandsPanel.SetActive(true);
    }

    public void InitializeCommandsPanel()
    {
        HideCollapseCommandsButton();
        HideExtendedCommands();
        ShowExtendCommandsButton();
    }

    public void HideCommandsPanel()
    {
        commandsPanel.SetActive(false);
    }

    // Extend Commands Button
    public void ShowExtendCommandsButton()
    {
        extendCommandsButton.SetActive(true);
    }

    public void HideExtendCommandsButton()
    {
        extendCommandsButton.SetActive(false);
    }

    // Collapse Commands Button
    public void ShowCollapseCommandsButton()
    {
        collapseCommandsButton.SetActive(true);
    }

    public void HideCollapseCommandsButton()
    {
        collapseCommandsButton.SetActive(false);
    }

    // Extended Commands
    public void ShowExtendedCommands()
    {
        extendedCommands.SetActive(true);
    }

    public void HideExtendedCommands()
    {
        extendedCommands.SetActive(false);
    }

    // Helping functions
    
    private IEnumerator FadeIn(GameObject target, float fadeDuration)
    {
        CanvasGroup cg = target.GetComponent<CanvasGroup>();
        if (cg == null) cg = loot.AddComponent<CanvasGroup>();

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        cg.alpha = 1f;
    }

    private IEnumerator FadeOut(GameObject target, float fadeDuration)
    {
        CanvasGroup cg = target.GetComponent<CanvasGroup>();
        if (cg == null) cg = loot.AddComponent<CanvasGroup>();

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        cg.alpha = 0f;
    }
}
