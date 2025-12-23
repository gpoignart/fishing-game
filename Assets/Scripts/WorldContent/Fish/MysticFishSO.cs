using UnityEngine;

[CreateAssetMenu(menuName = "Fish/MysticFish")]
public class MysticFishSO : FishSO
{
    public override void Initialize()
    {
        this.fishName = "MysticFish";
        this.spawnMaps = new MapSO[] { GameManager.Instance.MapRegistry.arcaneLakeSO };
        this.spawnTimes = new TimeOfDaySO[] { GameManager.Instance.TimeOfDayRegistry.nightSO };
        this.drops = new IngredientSO[] { GameManager.Instance.IngredientRegistry.mysticEssenceSO };
        this.spawnChance = 3;
        this.catchingDifficulties = new FishCatchingDifficulty[]
        {
            new FishCatchingDifficulty(time: GameManager.Instance.TimeOfDayRegistry.nightSO, safeZoneMoveSpeed: 190f, requiredTimeInsideZone: 3.2f, allowedTimeOutsideZone: 3f, safeZoneWidth: 80f)
        };
    }
}