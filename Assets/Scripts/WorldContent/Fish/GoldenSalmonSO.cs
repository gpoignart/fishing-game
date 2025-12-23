using UnityEngine;

[CreateAssetMenu(menuName = "Fish/GoldenSalmon")]
public class GoldenSalmonSO : FishSO
{
    public override void Initialize()
    {
        this.fishName = "GoldenSalmon";
        this.spawnMaps = new MapSO[] { GameManager.Instance.MapRegistry.driftwoodMarshSO };
        this.spawnTimes = new TimeOfDaySO[] { GameManager.Instance.TimeOfDayRegistry.daySO, GameManager.Instance.TimeOfDayRegistry.nightSO };
        this.drops = new IngredientSO[] { GameManager.Instance.IngredientRegistry.glimmeringScaleSO };
        this.spawnChance = 45;
        this.catchingDifficulties = new FishCatchingDifficulty[]
        {
            new FishCatchingDifficulty(time: GameManager.Instance.TimeOfDayRegistry.daySO, safeZoneMoveSpeed: 130f, requiredTimeInsideZone: 2.5f,allowedTimeOutsideZone: 3f, safeZoneWidth: 130f),
            new FishCatchingDifficulty(time: GameManager.Instance.TimeOfDayRegistry.nightSO, safeZoneMoveSpeed: 150f, requiredTimeInsideZone: 3f,allowedTimeOutsideZone: 3f, safeZoneWidth: 130f)
        };
    }
}