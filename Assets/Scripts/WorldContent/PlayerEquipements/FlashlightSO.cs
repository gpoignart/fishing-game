using UnityEngine;

[CreateAssetMenu(menuName = "PlayerEquipment/Flashlight")]
public class FlashlightSO : PlayerEquipmentSO
{
    public override void Initialize()
    {
        this.equipmentName = "Flashlight";
        this.level = 1;
        this.detailsPerLevel = new string[]
        {
            "No bonus",
            "...",
            "..."
        };
    }

    public override void UpgradeTo(int newLevel)
    {
        this.level = newLevel;
        if (this.level == 2)
        {
            
        }
        else if (this.level == 3)
        {
            
        }
    }
}