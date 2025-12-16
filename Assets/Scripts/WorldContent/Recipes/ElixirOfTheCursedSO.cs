using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/ElixirOfTheCursed")]
public class ElixirOfTheCursedSO : RecipeSO
{
    public override void Initialize()
    {
        this.recipeName = "Elixir of the Cursed";
        this.description = "Heals any curse.";
        this.ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.glimmeringScaleSO, quantity: 10),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.shadowingEyeSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.mysticEssenceSO, quantity: 3)
        };
        this.hasAlreadyBeenUsed = false;
        this.isFinalRecipe = true;
    }
}
