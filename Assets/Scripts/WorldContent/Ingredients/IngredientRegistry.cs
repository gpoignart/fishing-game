using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class IngredientRegistry
{
    public CarpMeatSO carpMeatSO;
    public CarpToothSO carpToothSO;
    public TroutMeatSO troutMeatSO;
    public ShinyFinSO shinyFinSO;
    public GlimmeringScaleSO glimmeringScaleSO;
    public ShadowingEyeSO shadowingEyeSO;
    public MysticEssenceSO mysticEssenceSO;

    // List of ingredients
    public IngredientSO[] AllIngredients =>
        new IngredientSO[]
        {
            carpMeatSO,
            carpToothSO,
            troutMeatSO,
            shinyFinSO,
            glimmeringScaleSO,
            shadowingEyeSO,
            mysticEssenceSO
        };

    public void Initialize()
    {
        carpMeatSO.Initialize();
        carpToothSO.Initialize();
        troutMeatSO.Initialize();
        shinyFinSO.Initialize();
        glimmeringScaleSO.Initialize();
        shadowingEyeSO.Initialize();
        mysticEssenceSO.Initialize();
    }

    public IngredientSO GetByName(string ingredientName)
    {
        return AllIngredients.FirstOrDefault(i => i.ingredientName == ingredientName);
    }

    public List<IngredientSO> GetAvailableIngredientsFromMap(MapSO map, TimeOfDaySO currentTime)
    {
        List<IngredientSO> ingredients = new List<IngredientSO>();

        FishSO[] allFish = GameManager.Instance.FishRegistry.AllFish;

        foreach (var fish in allFish)
        {
            if (!fish.spawnMaps.Contains(map)) continue;

            bool timeMatch = fish.catchingDifficulties.Any(diff => diff.time == currentTime);
            if (!timeMatch) continue;

            foreach (var ingredient in fish.drops)
            {
                if (!ingredients.Contains(ingredient))
                    ingredients.Add(ingredient);
            }
        }

        return ingredients;
    }
}
