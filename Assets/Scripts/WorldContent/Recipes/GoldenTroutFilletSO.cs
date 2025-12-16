using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/GoldenTroutFillet")]
public class GoldenTroutFilletSO : RecipeSO
{
    public override void Initialize()
    {
        this.recipeName = "Golden Trout Fillet";
        this.description = "Upgrades Flashlight to level 3:\n" + GameManager.Instance.PlayerEquipmentRegistry.flashlightSO.detailsPerLevel[2];
        this.ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.troutMeatSO, quantity: 10),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.shinyFinSO, quantity: 10),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.glimmeringScaleSO, quantity: 5)
        };
        this.hasAlreadyBeenUsed = false;
        this.isFinalRecipe = false;
        this.upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.flashlightSO;
        this.upgradesToLevel = 3;
    }
}
