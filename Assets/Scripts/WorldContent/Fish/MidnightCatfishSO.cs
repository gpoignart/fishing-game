using UnityEngine;

[CreateAssetMenu(menuName = "Fish/MidnightCatfish")]
public class MidnightCatfishSO : FishSO
{
    public override void Initialize()
    {
        this.fishName = "MidnightCatfish";
        this.spawnMaps = new MapSO[] { GameManager.Instance.MapRegistry.shadowmoonRiverSO };
        this.spawnTimes = new TimeOfDaySO[] { GameManager.Instance.TimeOfDayRegistry.nightSO };
        this.drops = new IngredientSO[] { GameManager.Instance.IngredientRegistry.shadowingEyeSO };
        this.spawnChance = 8;
        this.catchingDifficulties = new FishCatchingDifficulty[]
        {
            new FishCatchingDifficulty(time: GameManager.Instance.TimeOfDayRegistry.nightSO, safeZoneMoveSpeed: 180f, requiredTimeInsideZone: 3.2f, allowedTimeOutsideZone: 3f, safeZoneWidth: 100f)
        };
    }
}