using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/FishermansMix")]
public class FishermansMixSO : RecipeSO
{
    public override void Initialize()
    {
        this.recipeName = "Fisherman's Mix";
        this.description = "Upgrades Boat to level 2:\n" + GameManager.Instance.PlayerEquipmentRegistry.boatSO.detailsPerLevel[1];
        this.ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpMeatSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpToothSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.troutMeatSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.shinyFinSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.glimmeringScaleSO, quantity: 1)
        };
        this.hasAlreadyBeenUsed = false;
        this.isFinalRecipe = false;
        this.upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.boatSO;
        this.upgradesToLevel = 2;
    }
}
