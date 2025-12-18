using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndScreenManager : MonoBehaviour
{
    // Allow to call EndScreenManager.Instance anywhere (singleton)
    public static EndScreenManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI endText;

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
        endText.text = "Congratulations, you finished the story in " + GameManager.Instance.DaysCount + " days.\nThe lake is calm. The curse is broken.\nYou could start again… try to do better, faster.\nBut think twice.\nStarting over would mean pulling the fisherman back into the curse.\nBack into the sleepless nights.\nBack into the waiting.\nMaybe, some endings should be left untouched.";
    }

    public void OnRestartButtonPressed()
    {
        GameManager.Instance.StartNewGame();
    }

    // Quit game
    public void OnQuitButtonPressed()
    {
        GameManager.Instance.QuitGame();
    }
}