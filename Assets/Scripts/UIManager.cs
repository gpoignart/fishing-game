using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Panels
    [SerializeField]
    private GameObject IdlePanel;

    [SerializeField]
    private GameObject WaitPanel;

    [SerializeField]
    private GameObject HookPanel;

    [SerializeField]
    private GameObject DragPanel;

    [SerializeField]
    private GameObject TimeOutPanel;

    // Global UI elements
    [SerializeField]
    private GameObject BackgroundImage;

    [SerializeField]
    private GameObject Timer;

    [SerializeField]
    private GameObject FishCounter;

    [SerializeField]
    private GameObject FishBasicImage;

    [SerializeField]
    private GameObject FishWorriedImage;

    [SerializeField]
    private GameObject FishAngryImage;

    [SerializeField]
    private GameObject Character;

    [SerializeField]
    private GameObject CharacterWaitingImage;

    [SerializeField]
    private GameObject CharacterCatchingImage;

    // Hook Button (usefull to set its position randomly)
    [SerializeField]
    private GameObject HookButton;

    // FishCounter parameters (for moving it in the TimeOutPanel)
    [SerializeField]
    private Transform fishCounterOriginalParent;
    private Vector2 FishCounterCenterPosition = new Vector2(0, 0);
    private Vector2 FishCounterTopRightPosition = new Vector2(760, 430);

    // Texts to update for the timer and the fishcount
    [SerializeField]
    private TMPro.TextMeshProUGUI timerText;

    [SerializeField]
    private TMPro.TextMeshProUGUI fishCounterText;

    // Called when the game start, initialize the UI
    void Start()
    {
        CharacterCatchingImage.SetActive(false);
        FishAngryImage.SetActive(false);
        FishWorriedImage.SetActive(false);
        // We start on the idle panel
        ShowIdlePanel();
    }

    // This method shows the idle panel
    public void ShowIdlePanel()
    {
        // Set inactive all panels except the idle panel
        IdlePanel.SetActive(true);
        WaitPanel.SetActive(false);
        HookPanel.SetActive(false);
        DragPanel.SetActive(false);
        TimeOutPanel.SetActive(false);

        // Set in-game elements active
        SetInGameElementsActive();
    }

    // This method shows the waiting panel
    public void ShowWaitPanel()
    {
        // Set inactive all panels except the wait panel
        IdlePanel.SetActive(false);
        WaitPanel.SetActive(true);
        HookPanel.SetActive(false);
        DragPanel.SetActive(false);
        TimeOutPanel.SetActive(false);

        // Set in-game elements active
        SetInGameElementsActive();
    }

    // This method shows the hook panel
    public void ShowHookPanel()
    {
        // Set inactive all panels except the hook panel
        IdlePanel.SetActive(false);
        WaitPanel.SetActive(false);
        HookPanel.SetActive(true);
        DragPanel.SetActive(false);
        TimeOutPanel.SetActive(false);

        // Set in-game elements active
        SetInGameElementsActive();

        // Set the hook button position randomly in the 3/4 from the bottom of the screen
        SetHookButtonRandomPosition();
    }

    // This method shows the drag panel
    public void ShowDragPanel()
    {
        // Set inactive all panels except the drag panel
        IdlePanel.SetActive(false);
        WaitPanel.SetActive(false);
        HookPanel.SetActive(false);
        DragPanel.SetActive(true);
        TimeOutPanel.SetActive(false);

        // Set in-game elements active
        SetInGameElementsActive();
        FishBasicImage.SetActive(false);
        FishWorriedImage.SetActive(true);
    }

    // This method shows the timeout panel
    public void ShowTimeOutPanel()
    {
        // Set inactive all panels except the timeout panel
        IdlePanel.SetActive(false);
        WaitPanel.SetActive(false);
        HookPanel.SetActive(false);
        DragPanel.SetActive(false);
        TimeOutPanel.SetActive(true);

        // Set the fish counter at the center position
        FishCounter.transform.SetParent(TimeOutPanel.transform, false);
        RectTransform FishCounterRect = FishCounter.GetComponent<RectTransform>();
        FishCounterRect.anchoredPosition = FishCounterCenterPosition;
        FishCounter.SetActive(true);

        // Set in-game elements inactive
        Timer.SetActive(false);
        Character.SetActive(false);
    }

    // This method updates the timer display
    public void UpdateTimerDisplay(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    // This method change the timer text color
    public void SetTimerColor(Color c)
    {
        if (timerText)
            timerText.color = c;
    }

    // This method updates the fish count display
    public void UpdateFishCounterDisplay(int fishCount)
    {
        fishCounterText.text = "x " + fishCount.ToString();
    }

    public void PlayCatchAnimation()
    {
        StartCoroutine(StartCatchCoroutine());
    }

    private IEnumerator StartCatchCoroutine()
    {
        CharacterCatchingImage.SetActive(true);
        CharacterWaitingImage.SetActive(false);
        FishBasicImage.SetActive(false);
        FishAngryImage.SetActive(true);

        yield return new WaitForSeconds(1f);

        CharacterCatchingImage.SetActive(false);
        CharacterWaitingImage.SetActive(true);
        FishAngryImage.SetActive(false);
        FishBasicImage.SetActive(true);
    }

    // This method sets in-game ui element active and positionate top-right the fish counter
    private void SetInGameElementsActive()
    {
        // Set the fish counter at the top-right position
        FishCounter.transform.SetParent(fishCounterOriginalParent, false);
        RectTransform FishCounterRect = FishCounter.GetComponent<RectTransform>();
        FishCounterRect.anchoredPosition = FishCounterTopRightPosition;

        // Active elements
        Timer.SetActive(true);
        FishCounter.SetActive(true);
        Character.SetActive(true);
        FishBasicImage.SetActive(true);
        FishWorriedImage.SetActive(false);
    }

    // This method sets the hookButton at a random position in the canvas (in the 3/4 from the bottom)
    private void SetHookButtonRandomPosition()
    {
        RectTransform HookPanelRect = HookPanel.GetComponent<RectTransform>();
        RectTransform HookButtonRect = HookButton.GetComponent<RectTransform>();

        // Horizontal limits (all the width)
        float xMin = -HookPanelRect.rect.width / 2 + HookButtonRect.rect.width / 2;
        float xMax = HookPanelRect.rect.width / 2 - HookButtonRect.rect.width / 2;

        // Vertical limits (the 3/4 from the bottom)
        float yMin = -HookPanelRect.rect.height / 2 + HookButtonRect.rect.height / 2;
        float yMax =
            -HookPanelRect.rect.height / 2
            + HookPanelRect.rect.height * 0.75f
            - HookButtonRect.rect.height / 2;

        // Position al�atoire
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);

        HookButtonRect.anchoredPosition = new Vector2(randomX, randomY);
    }
}
