using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishSO fishSO;
    private SpriteRenderer sr;

    // Called each time a new game object fish is instanciated
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = fishSO.sprite;
    }
}
