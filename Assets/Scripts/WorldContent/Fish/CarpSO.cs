using UnityEngine;

[CreateAssetMenu(menuName = "Fish/Carp")]
public class CarpSO : FishSO
{
    public override void Initialize()
    {
        this.fishName = "Carp";
        this.spawnMaps = new MapSO[] { GameManager.Instance.MapRegistry.shadowmoonRiverSO, GameManager.Instance.MapRegistry.driftwoodMarshSO };
        this.spawnTimes = new TimeOfDaySO[] { GameManager.Instance.TimeOfDayRegistry.daySO, GameManager.Instance.TimeOfDayRegistry.nightSO };
        this.drops = new IngredientSO[] { GameManager.Instance.IngredientRegistry.carpMeatSO, GameManager.Instance.IngredientRegistry.carpToothSO };
        this.spawnChance = 80;
        this.catchingDifficulties = new FishCatchingDifficulty[]
        {
            new FishCatchingDifficulty(time: GameManager.Instance.TimeOfDayRegistry.daySO, safeZoneMoveSpeed: 80f, requiredTimeInsideZone: 2f,allowedTimeOutsideZone: 3f, safeZoneWidth: 200f),
            new FishCatchingDifficulty(time: GameManager.Instance.TimeOfDayRegistry.nightSO, safeZoneMoveSpeed: 100f, requiredTimeInsideZone: 2.5f,allowedTimeOutsideZone: 2.5f, safeZoneWidth: 200f)
        };
    }
}