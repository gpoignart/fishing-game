[System.Serializable]
public class RecipeRegistry
{
    public CarpSkilletSO carpSkilletSO;
    public TroutSkilletSO troutSkilletSO;
    public FishermansMixSO fishermansMixSO;
    public GoldenCarpFilletSO goldenCarpFilletSO;
    public GoldenTroutFilletSO goldenTroutFilletSO;
    public CatfishNightStewSO catfishNightStewSO;
    public ElixirOfTheCursedSO elixirOfTheCursedSO;

    // List of recipes to display them more easily in recipe book
    public RecipeSO[] AllRecipes =>
        new RecipeSO[]
        {
            carpSkilletSO,
            troutSkilletSO,
            fishermansMixSO,
            goldenCarpFilletSO,
            goldenTroutFilletSO,
            catfishNightStewSO,
            elixirOfTheCursedSO
        };

    public void Initialize()
    {
        carpSkilletSO.Initialize();
        troutSkilletSO.Initialize();
        fishermansMixSO.Initialize();
        goldenCarpFilletSO.Initialize();
        goldenTroutFilletSO.Initialize();
        catfishNightStewSO.Initialize();
        elixirOfTheCursedSO.Initialize();
    }
}
