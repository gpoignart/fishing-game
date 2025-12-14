using UnityEngine;

public abstract class MapSO : ScriptableObject
{
    public string mapName;
    // Day sprites
    public Sprite skyDaySprite;
    public Color skyDayColor; // To remove when we'll got real sprite
    public Sprite underwaterDaySprite;
    public Color underwaterDayColor; // To remove when we'll got real sprite
    public Sprite lakeFloorDaySprite;
    public Color lakeFloorDayColor; // To remove when we'll got real sprite
    // Night sprites    
    public Sprite skyNightSprite;
    public Color skyNightColor; // To remove when we'll got real sprite
    public Sprite underwaterNightSprite;
    public Color underwaterNightColor; // To remove when we'll got real sprite
    public Sprite lakeFloorNightSprite;
    public Color lakeFloorNightColor; // To remove when we'll got real sprite

    public abstract void Initialize();
}

