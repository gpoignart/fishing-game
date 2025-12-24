using UnityEngine;

[CreateAssetMenu(menuName = "PlayerEquipment/Flashlight")]
public class FlashlightSO : PlayerEquipmentSO
{
    public Vector2 beamSize;
    public Vector2 beamTimerSize;
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
        this.beamSize = new Vector2(100f, 100f);
        this.beamTimerSize = beamSize * 1.17f;
        this.beamFollowSpeed = 1f;
    }

    public override void UpgradeTo(int newLevel)
    {
        this.level = newLevel;
        if (this.level == 2)
        {
            // Increase of 50% the beam size and timer size
            this.beamSize *= 1.5f;
            this.beamTimerSize *= 1.5f;
        }
        else if (this.level == 3)
        {
            // Increase the speed of the beam
            this.beamFollowSpeed *= 4f;
        }
    }
}