using UnityEngine;

public abstract class FishSO : ScriptableObject
{
    public string fishName;
    public Sprite sprite;
    public MapSO[] spawnMaps;
    public TimeOfDaySO[] spawnTimes;
    public IngredientSO[] drops;
    public int spawnChance;
    public float staySpawnedTime;
    public FishCatchingDifficulty[] catchingDifficulties;

    public abstract void Initialize();
}

[System.Serializable] // Needed for an intern class
public class FishCatchingDifficulty
{
    public TimeOfDaySO time; // Day or night

    public float safeZoneMoveSpeed;
    public float requiredTimeInsideZone;
    public float allowedTimeOutsideZone;
    public float safeZoneWidth;

    // Constructor
    public FishCatchingDifficulty(TimeOfDaySO time, float safeZoneMoveSpeed, float requiredTimeInsideZone, float allowedTimeOutsideZone, float safeZoneWidth)
    {
        this.time = time;
        this.safeZoneMoveSpeed = safeZoneMoveSpeed;
        this.requiredTimeInsideZone = requiredTimeInsideZone;
        this.allowedTimeOutsideZone = allowedTimeOutsideZone;
        this.safeZoneWidth = safeZoneWidth;
    }
}