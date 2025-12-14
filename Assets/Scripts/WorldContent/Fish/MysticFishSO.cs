using UnityEngine;

[CreateAssetMenu(menuName = "FishType/MysticFish")]
public class MysticFishSO : FishSO
{
    public override void Initialize()
    {
        this.fishName = "MysticFish";
        this.spawnMaps = new MapSO[] { GameManager.Instance.MapRegistry.arcaneLakeSO };
        this.spawnTimes = new TimeOfDaySO[] { GameManager.Instance.TimeOfDayRegistry.nightSO };
        this.drops = new IngredientSO[] { GameManager.Instance.IngredientRegistry.mysticEssenceSO };
        this.spawnChance = 5;
        this.catchingDifficulties = new FishCatchingDifficulty[]
        {
            new FishCatchingDifficulty(time: GameManager.Instance.TimeOfDayRegistry.nightSO, safeZoneMoveSpeed: 200f, requiredTimeInsideZone: 3f, allowedTimeOutsideZone: 2f, safeZoneWidth: 80f)
        };
    }
}