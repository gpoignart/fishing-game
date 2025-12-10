using UnityEngine;

[CreateAssetMenu(menuName = "TimeOfDay/Day")]
public class DaySO : TimeOfDaySO
{
    public override void Initialize()
    {
        this.timeOfDayName = "Day";
        this.duration = 30f;
    }
}
