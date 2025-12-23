using UnityEngine;

[CreateAssetMenu(menuName = "PlayerEquipment/Flashlight")]
public class FlashlightSO : PlayerEquipmentSO
{
    public Vector2 beamSize;
    public float beamFollowSpeed;

    public override void Initialize()
    {
        this.playerEquipmentName = "Flashlight";
        this.level = 1;
        this.detailsPerLevel = new string[]
        {
            "No bonus",
            "Increases beam's size",
            "Increases beam's speed"
        };
        this.beamSize = new Vector2(130f, 130f);
        this.beamFollowSpeed = 1.5f;
    }

    public override void UpgradeTo(int newLevel)
    {
        this.level = newLevel;
        if (this.level == 2)
        {
            // Increase of 50% the beam size
            this.beamSize *= 1.5f;
        }
        else if (this.level == 3)
        {
            // Increase the speed of the beam
            this.beamFollowSpeed *= 4f;
        }
    }
}