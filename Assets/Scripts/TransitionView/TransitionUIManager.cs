using UnityEngine;
using TMPro;

public class TransitionUIManager : MonoBehaviour
{
    // Allow to call TransitionUIManager.Instance anywhere (singleton)
    public static TransitionUIManager Instance { get; private set; }

    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
    [SerializeField] private TextMeshProUGUI transitionText;

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
        backgroundSpriteRenderer.sprite = GameManager.Instance.CurrentTransition.backgroundSprite;
        backgroundSpriteRenderer.color = GameManager.Instance.CurrentTransition.backgroundColor;
        transitionText.text = GameManager.Instance.CurrentTransition.text;
    }
}
