using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    // Allow to call CreditsManager.Instance anywhere (singleton)
    public static CreditsManager Instance { get; private set; }

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

    public void OnExitButtonPressed()
    {
        GameManager.Instance.ExitCredits();
        AudioManager.Instance.PlayPressingButtonSFX();
    }
}