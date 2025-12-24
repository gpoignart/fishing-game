using UnityEngine;

public abstract class MapSO : ScriptableObject
{
    public string mapName;
    public Sprite dayBottomBackgroundSprite;
    public Sprite dayTopBackgroundSprite;
    public Sprite nightBottomBackgroundSprite;
    public Sprite nightTopBackgroundSprite;
    public Sprite dayLogoSprite;
    public Sprite nightLogoSprite;
    public Color dayBubbleColor;
    public Color nightBubbleColor;

    public abstract void Initialize();
}

