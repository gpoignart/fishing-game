using UnityEngine;

[CreateAssetMenu(menuName = "Fish/Trout")]
public class TroutSO : FishSO
{
    public override void Initialize()
    {
        this.fishName = "Trout";
        this.spawnMaps = new MapSO[] { GameManager.Instance.MapRegistry.driftwoodMarshSO, GameManager.Instance.MapRegistry.arcaneLakeSO };
        this.spawnTimes = new TimeOfDaySO[] { GameManager.Instance.TimeOfDayRegistry.daySO, GameManager.Instance.TimeOfDayRegistry.nightSO };
        this.drops = new IngredientSO[] { GameManager.Instance.IngredientRegistry.troutMeatSO, GameManager.Instance.IngredientRegistry.shinyFinSO };
        this.spawnChance = 70;
        this.catchingDifficulties = new FishCatchingDifficulty[]
        {
            new FishCatchingDifficulty(time: GameManager.Instance.TimeOfDayRegistry.daySO, safeZoneMoveSpeed: 100f, requiredTimeInsideZone: 1.5f,allowedTimeOutsideZone: 3f, safeZoneWidth: 170f),
            new FishCatchingDifficulty(time: GameManager.Instance.TimeOfDayRegistry.nightSO, safeZoneMoveSpeed: 120f, requiredTimeInsideZone: 2f,allowedTimeOutsideZone: 3f, safeZoneWidth: 170f)
        };
    }
}