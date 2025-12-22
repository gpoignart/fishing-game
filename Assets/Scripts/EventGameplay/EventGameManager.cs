using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EventGameManager : MonoBehaviour
{
    // Allow to call EventGameManager.Instance anywhere (singleton)
    public static EventGameManager Instance { get; private set; }

    // Internal attributes
    private int indexOfText;
    private int numberOfLines;

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
        indexOfText = 0;
        numberOfLines = GameManager.Instance.CurrentEvent.eventLines.Length;   

        EventUIManager.Instance.UpdateText(GameManager.Instance.CurrentEvent.eventLines[indexOfText].text);
        EventUIManager.Instance.UpdateBackgroundImage(GameManager.Instance.CurrentEvent.eventLines[indexOfText].backgroundImage);
    }

    private void Update()
    {
        // Click on next button with return
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnNextButtonPressed();
        }
    }

    // Called when the player clicks next button
    public void OnNextButtonPressed()
    {
        indexOfText ++;
        
        if (indexOfText >= numberOfLines)
        {
            GameManager.Instance.ExitEvent();
        }
        else
        {
            EventUIManager.Instance.UpdateText(GameManager.Instance.CurrentEvent.eventLines[indexOfText].text);
            EventUIManager.Instance.UpdateBackgroundImage(GameManager.Instance.CurrentEvent.eventLines[indexOfText].backgroundImage);
        }
    }
}