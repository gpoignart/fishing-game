using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/CatfishNightStew")]
public class CatfishNightStewSO : RecipeSO
{
    public override void Initialize()
    {
        recipeName = "Catfish Night Stew";
        description = "Upgrades Boat to level 3.";
        ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpMeatSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpToothSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.troutMeatSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.shinyFinSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.shadowingEyeSO, quantity: 1)
        };
        hasAlreadyBeenUsed = false;
        isFinalRecipe = false;
        upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.boatSO;
        upgradesToLevel = 3;
    }
}
