using UnityEngine;

[CreateAssetMenu(menuName = "FishType/MidnightCatfish")]
public class MidnightCatfishSO : FishSO
{
    public override void Initialize()
    {
        this.fishName = "MidnightCatfish";
        this.spawnMaps = new MapSO[] { GameManager.Instance.MapRegistry.shadowmoonMarshSO };
        this.spawnTimes = new TimeOfDaySO[] { GameManager.Instance.TimeOfDayRegistry.nightSO };
        this.drops = new IngredientSO[] { GameManager.Instance.IngredientRegistry.shadowingEyeSO };
        this.spawnChance = 15;
        this.catchingDifficulties = new FishCatchingDifficulty[]
        {
            new FishCatchingDifficulty(time: GameManager.Instance.TimeOfDayRegistry.nightSO, safeZoneMoveSpeed: 170f, requiredTimeInsideZone: 3f,allowedTimeOutsideZone: 2.5f, safeZoneWidth: 100f)
        };
    }
}