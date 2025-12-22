using UnityEngine;

public abstract class MapSO : ScriptableObject
{
    public string mapName;
    public Sprite dayBackgroundSprite;
    public Sprite nightBackgroundSprite;

    public abstract void Initialize();
}

