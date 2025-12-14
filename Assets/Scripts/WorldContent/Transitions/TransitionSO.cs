using UnityEngine;

public abstract class TransitionSO : ScriptableObject
{
    public Sprite backgroundSprite;
    public Color backgroundColor;
    public string text;
    public float duration;
    public GameManager.GameState nextGameState;

    public abstract void Initialize();
}
