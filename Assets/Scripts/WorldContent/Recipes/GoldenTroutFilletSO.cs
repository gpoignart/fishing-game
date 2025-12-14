using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/GoldenTroutFillet")]
public class GoldenTroutFilletSO : RecipeSO
{
    public override void Initialize()
    {
        recipeName = "Golden Trout Fillet";
        description = "Upgrades Flashlight to level 3.";
        ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.troutMeatSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.shinyFinSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.glimmeringScaleSO, quantity: 3)
        };
        hasAlreadyBeenUsed = false;
        isFinalRecipe = false;
        upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.flashlightSO;
        upgradesToLevel = 3;
    }
}
