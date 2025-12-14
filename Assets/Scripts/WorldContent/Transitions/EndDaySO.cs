using UnityEngine;

[CreateAssetMenu(menuName = "Transition/EndDay")]
public class EndDaySO : TransitionSO
{
    public override void Initialize()
    {
        this.text = "The day has ended...";
        this.duration = 2f;
        this.nextGameState = GameManager.GameState.MapSelection;
    }
}
