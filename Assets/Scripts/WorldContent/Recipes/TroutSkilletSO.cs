using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/TroutSkillet")]
public class TroutSkilletSO : RecipeSO
{
    public override void Initialize()
    {
        recipeName = "Trout Skillet";
        description = "Upgrades Flashlight to level 2.";
        ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.troutMeatSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.shinyFinSO, quantity: 5)
        };
        hasAlreadyBeenUsed = false;
        isFinalRecipe = false;
        upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.flashlightSO;
        upgradesToLevel = 2;
    }
}
