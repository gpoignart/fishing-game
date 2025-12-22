using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class EventUIManager : MonoBehaviour
{
    // Allow to call EventUIManager.Instance anywhere (singleton)
    public static EventUIManager Instance { get; private set; }

    // Global UI elements
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI text;

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
    
    public void UpdateBackgroundImage(Sprite backgroundSprite)
    {
        backgroundImage.sprite = backgroundSprite;
    }

    public void UpdateText(string textContent)
    {
        text.text = textContent;
    }
}
