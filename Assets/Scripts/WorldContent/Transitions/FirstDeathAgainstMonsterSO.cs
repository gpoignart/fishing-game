using UnityEngine;

[CreateAssetMenu(menuName = "Transition/FirstDeathAgainstMonster")]
public class FirstDeathAgainstMonsterSO : TransitionSO
{
    public override void Initialize()
    {
        this.text = "You need to be faster...\nThat monster almost got you.\nOut of options, you face it again.";
        this.duration = 3.5f;
        this.nextGameState = GameManager.GameState.MonsterView;
    }
}
