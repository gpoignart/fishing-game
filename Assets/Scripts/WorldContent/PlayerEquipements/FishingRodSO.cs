using UnityEngine;

[CreateAssetMenu(menuName = "PlayerEquipment/FishingRod")]
public class FishingRodSO : PlayerEquipmentSO
{
    public override void Initialize()
    {
        this.equipmentName = "Fishing Rod";
        this.level = 1;
        this.detailsPerLevel = new string[]
        {
            "No bonus",
            "Increases green zone width",
            "Decreases time inside green zone needed"
        };
    }

    public override void UpgradeTo(int newLevel)
    {
        this.level = newLevel;
        if (this.level == 2)
        {
            // Increases the safeZoneWidth of 50% for all fish
            foreach (FishSO fish in GameManager.Instance.FishRegistry.AllFish)
            {
                foreach (FishCatchingDifficulty catchingDifficulty in fish.catchingDifficulties)
                {
                    catchingDifficulty.safeZoneWidth *= 1.5f;
                }
            }
        }
        else if (this.level == 3)
        {
            // Decreases the requiredTimeInsideZone of 30% for all fish
            foreach (FishSO fish in GameManager.Instance.FishRegistry.AllFish)
            {
                foreach (FishCatchingDifficulty catchingDifficulty in fish.catchingDifficulties)
                {
                    catchingDifficulty.requiredTimeInsideZone *= 0.7f;
                }
            }
        }
    }
}

