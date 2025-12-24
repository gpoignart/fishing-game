using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    // Singleton
    public static EventManager Instance { get; private set; }

    // Global UI elements
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI text;

    // Parameters
    private float delay = 0.03f;

    // Internal attributes
    private int indexOfText;
    private int numberOfLines;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    // Singleton setup
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
        indexOfText = 0;
        numberOfLines = GameManager.Instance.CurrentEvent.eventLines.Length;
        ShowText(indexOfText);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // Skip actual text
                StopCoroutine(typingCoroutine);
                text.text = GameManager.Instance.CurrentEvent.eventLines[indexOfText].text;
                isTyping = false;
            }
            else
            {
                OnNextButtonPressed();
            }
        }
    }

    private void ShowText(int textIndex)
    {
        var line = GameManager.Instance.CurrentEvent.eventLines[textIndex];
        backgroundImage.sprite = line.backgroundImage;

        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    private IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        text.text = "";

        foreach (char c in fullText)
        {
            text.text += c;
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
    }

    // Called when the player clicks next button or presses Return
    public void OnNextButtonPressed()
    {
        indexOfText++;

        if (indexOfText >= numberOfLines)
        {
            GameManager.Instance.ExitEvent();
        }
        else
        {
            ShowText(indexOfText);
        }
    }
}
