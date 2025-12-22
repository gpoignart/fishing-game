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
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    public void OnQuitButtonPressed()
    {
        GameManager.Instance.QuitGame();
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    public void OnCreditsButtonPressed()
    {
        GameManager.Instance.EnterCredits();
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    // Start a new game button / popup

    public void OnNewGameButtonPressed()
    {
        if (SaveSystem.HasSave())
        {
            DisableMainMenuButtons();
            ShowConfirmationPopUp();
        }
        else
        {
            GameManager.Instance.StartNewGame();
        }
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    public void OnPopUpCancelButton()
    {
        AbleMainMenuButtons();
        HideConfirmationPopUp();
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    public void OnPopUpYesButton()
    {
        GameManager.Instance.StartNewGame();
        AudioManager.Instance.PlayPressingButtonSFX();
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