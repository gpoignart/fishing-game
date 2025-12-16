using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/TroutSkillet")]
public class TroutSkilletSO : RecipeSO
{
    public override void Initialize()
    {
        this.recipeName = "Trout Skillet";
        this.description = "Upgrades Flashlight to level 2:\n" + GameManager.Instance.PlayerEquipmentRegistry.flashlightSO.detailsPerLevel[1];
        this.ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.troutMeatSO, quantity: 10),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.shinyFinSO, quantity: 10)
        };
        this.hasAlreadyBeenUsed = false;
        this.isFinalRecipe = false;
        this.upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.flashlightSO;
        this.upgradesToLevel = 2;
    }
}
