using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishRegistry
{
    public CarpSO carpSO;
    public TroutSO troutSO;
    public GoldenSalmonSO goldenSalmonSO;
    public MidnightCatfishSO midnightCatfishSO;
    public MysticFishSO mysticFishSO;

    // List of fish
    public FishSO[] AllFish =>
        new FishSO[]
        {
            carpSO,
            troutSO,
            goldenSalmonSO,
            midnightCatfishSO,
            mysticFishSO
        };
    
    // Dictionnary ingredient to fish
    private Dictionary<IngredientSO, FishSO> ingredientToFish = new Dictionary<IngredientSO, FishSO>();

    public void Initialize()
    {
        carpSO.Initialize();
        troutSO.Initialize();
        goldenSalmonSO.Initialize();
        midnightCatfishSO.Initialize();
        mysticFishSO.Initialize();
        BuildIngredientToFish();
    }

    // Build dictionary ingredient to fish
    private void BuildIngredientToFish()
    {
        ingredientToFish.Clear();

        foreach (FishSO fish in AllFish)
        {
            foreach (IngredientSO ingredient in fish.drops)
            {
                if (ingredientToFish.ContainsKey(ingredient))
                {
                    continue;
                }

                ingredientToFish.Add(ingredient, fish);
            }
        }
    }

    public FishSO GetFishFromIngredient(IngredientSO ingredient)
    {
        if (ingredientToFish.TryGetValue(ingredient, out FishSO fish)) { return fish; }
        return null;
    }
}

