using System.Collections;
using UnityEngine;

public class TransitionGameManager : MonoBehaviour
{
    // Allow to call TransitionGameManager.Instance anywhere (singleton)
    public static TransitionGameManager Instance { get; private set; }

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
        StartCoroutine(PlayTransition(GameManager.Instance.CurrentTransition));
    }

    public IEnumerator PlayTransition(TransitionSO transition)
    {        
        yield return new WaitForSeconds(transition.duration);
        GameManager.Instance.ExitTransition();
    }
}