using UnityEngine;

[CreateAssetMenu(menuName = "TimeOfDay/Night")]
public class NightSO : TimeOfDaySO
{
    public override void Initialize()
    {
        this.timeOfDayName = "Night";
        this.duration = 60f;
    }
}
