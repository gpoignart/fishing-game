[System.Serializable]
public class TransitionRegistry
{
    public EndDaySO endDaySO;
    public EndNightSO endNightSO;
    public FirstDeathAgainstMonsterSO firstDeathAgainstMonsterSO;
    public DeathAgainstMonsterSO deathAgainstMonsterSO;

    public void Initialize()
    {
        endDaySO.Initialize();
        endNightSO.Initialize();
        firstDeathAgainstMonsterSO.Initialize();
        deathAgainstMonsterSO.Initialize();
    }
}
