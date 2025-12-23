using UnityEngine;

[CreateAssetMenu(menuName = "PlayerEquipment/Boat")]
public class BoatSO : PlayerEquipmentSO
{
    public float speed;

    public override void Initialize()
    {
        this.playerEquipmentName = "Boat";
        this.level = 1;
        this.detailsPerLevel = new string[]
        {
            "No bonus",
            "Increases boat speed",
            "Increases chances of rare fish appearing"
        };
        this.speed = 2f;
    }

    public override void UpgradeTo(int newLevel)
    {
        this.level = newLevel;
        if (this.level == 2)
        {
            // Increases the boat speed of 50%
            this.speed *= 1.5f;
        }
        else if (this.level == 3)
        {
            // Add 12% of spawn chance for rare fish
            GameManager.Instance.FishRegistry.midnightCatfishSO.spawnChance += 12;
            GameManager.Instance.FishRegistry.mysticFishSO.spawnChance += 12;
        }
    }
}