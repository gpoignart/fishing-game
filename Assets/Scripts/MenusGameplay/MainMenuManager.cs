using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    // Allow to call MainMenuManager.Instance anywhere (singleton)
    public static MainMenuManager Instance { get; private set; }

    [SerializeField] private Button continueButton;
    [SerializeField] private CanvasGroup mainMenuButtons;
    [SerializeField] private GameObject confirmationPopUp;

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
        continueButton.interactable = SaveSystem.HasSave();
        HideConfirmationPopUp();
    }

    // Main buttons

    public void OnContinueButtonPressed()
    {
        GameManager.Instance.ContinueGame();
    }

    public void OnQuitButtonPressed()
    {
        GameManager.Instance.QuitGame();
    }

    public void OnCreditsButtonPressed()
    {
        GameManager.Instance.EnterCredits();
    }

    // Start a new game button / popup

    public void OnNewGameButtonPressed()
    {
        DisableMainMenuButtons();
        ShowConfirmationPopUp();
    }

    public void OnPopUpCancelButton()
    {
        AbleMainMenuButtons();
        HideConfirmationPopUp();
    }

    public void OnPopUpYesButton()
    {
        GameManager.Instance.StartNewGame();
    }

    // Main menu buttons UI

    private void AbleMainMenuButtons()
    {
        mainMenuButtons.interactable = true;
    }

    private void DisableMainMenuButtons()
    {
        mainMenuButtons.interactable = false;
    }

    // Confirmation popup UI

    private void ShowConfirmationPopUp()
    {
        confirmationPopUp.SetActive(true);
    }
    
    private void HideConfirmationPopUp()
    {
        confirmationPopUp.SetActive(false);
    }
}