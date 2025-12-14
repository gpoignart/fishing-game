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
    private SpriteRenderer skySpriteRenderer;

    [SerializeField]
    private SpriteRenderer underwaterSpriteRenderer;

    [SerializeField]
    private SpriteRenderer lakeFloorSpriteRenderer;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private GameObject hookButton;

    [SerializeField]
    private GameObject dragBar;

    [SerializeField]
    private GameObject loot;

    [SerializeField]
    private GameObject loseFishText;

    [SerializeField]
    private Image lootImage;

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
        hookButton.SetActive(false);
        dragBar.SetActive(false);
        loot.SetActive(false);
        loseFishText.SetActive(false);

        // Initialize backgrounds sprites
        if (GameManager.Instance.CurrentTimeOfDay == GameManager.Instance.TimeOfDayRegistry.daySO)
        {
            skySpriteRenderer.sprite = GameManager.Instance.CurrentMap.skyDaySprite;
            skySpriteRenderer.color = GameManager.Instance.CurrentMap.skyDayColor;
            underwaterSpriteRenderer.sprite = GameManager.Instance.CurrentMap.underwaterDaySprite;
            underwaterSpriteRenderer.color = GameManager.Instance.CurrentMap.underwaterDayColor;
            lakeFloorSpriteRenderer.sprite = GameManager.Instance.CurrentMap.lakeFloorDaySprite;
            lakeFloorSpriteRenderer.color = GameManager.Instance.CurrentMap.lakeFloorDayColor;
        }
        else
        {
            skySpriteRenderer.sprite = GameManager.Instance.CurrentMap.skyNightSprite;
            skySpriteRenderer.color = GameManager.Instance.CurrentMap.skyNightColor;
            underwaterSpriteRenderer.sprite = GameManager.Instance.CurrentMap.underwaterNightSprite;
            underwaterSpriteRenderer.color = GameManager.Instance.CurrentMap.underwaterNightColor;
            lakeFloorSpriteRenderer.sprite = GameManager.Instance.CurrentMap.lakeFloorNightSprite;
            lakeFloorSpriteRenderer.color = GameManager.Instance.CurrentMap.lakeFloorNightColor;
        }
    }

    // Update the timer display
    public void UpdateTimerUI(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    // Show the loot (start a corouting for showing the loot only for a time)
    public void ShowLoot(IngredientSO ingredient)
    {
        StartCoroutine(ShowLootForSeconds(ingredient, 1.5f));
    }

    private IEnumerator ShowLootForSeconds(IngredientSO ingredient, float duration)
    {
        loot.SetActive(true);
        lootImage.sprite = ingredient.sprite;
        lootImage.color = ingredient.color;

        yield return new WaitForSeconds(duration);

        loot.SetActive(false);
    }

    // Show the loseFishText (start a corouting for showing the loseFishText only for a time)
    public void ShowLoseFishText()
    {
        StartCoroutine(ShowLoseFishTextForSeconds(1.5f));
    }

    private IEnumerator ShowLoseFishTextForSeconds(float duration)
    {
        loseFishText.SetActive(true);

        yield return new WaitForSeconds(duration);

        loseFishText.SetActive(false);
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

    // Hide the UI for the fishing state
    public void HideFishingStateUI()
    {
        dragBar.SetActive(false);
    }
}
