using UnityEngine;

[CreateAssetMenu(menuName = "Transition/EndNight")]
public class EndNightSO : TransitionSO
{
    public override void Initialize()
    {
        this.text = "The night has finally passed...";
        this.duration = 3f;
    }
}
