using UnityEngine;

[CreateAssetMenu(menuName = "Transition/DeathAgainstMonster")]
public class DeathAgainstMonsterSO : TransitionSO
{
    public override void Initialize()
    {
        this.text = "You wake up, it's morning.\nLast night... that monster got you.\nEverything you caught yesterday is gone.";
        this.duration = 4f;
        this.nextGameState = GameManager.GameState.MapSelection;
    }
}
