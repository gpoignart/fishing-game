using UnityEngine;

public abstract class FishSO : ScriptableObject
{
    public string fishName;
    public Sprite sprite;
    public MapSO[] spawnMaps;
    public TimeOfDaySO[] spawnTimes;
    public IngredientSO[] drops;
    public int spawnChance;
    public FishCatchingDifficulty[] catchingDifficulties;

    public abstract void Initialize();
}

[System.Serializable] // Needed for an intern class
public class FishCatchingDifficulty
{
    public TimeOfDaySO time; // Day or night

    public float safeZoneMoveSpeed = 100f;
    public float requiredTimeInsideZone = 2f;
    public float allowedTimeOutsideZone = 3f;
    public float safeZoneWidth = 150f;

    // Default constructor
    public FishCatchingDifficulty(TimeOfDaySO time)
    {
        this.time = time;
    }

    // Constructor with difficulty parameters
    public FishCatchingDifficulty(TimeOfDaySO time, float safeZoneMoveSpeed, float requiredTimeInsideZone, float allowedTimeOutsideZone, float safeZoneWidth)
    {
        this.time = time;
        this.safeZoneMoveSpeed = safeZoneMoveSpeed;
        this.requiredTimeInsideZone = requiredTimeInsideZone;
        this.allowedTimeOutsideZone = allowedTimeOutsideZone;
        this.safeZoneWidth = safeZoneWidth;
    }
}