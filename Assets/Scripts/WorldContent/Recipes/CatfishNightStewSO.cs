using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/CatfishNightStew")]
public class CatfishNightStewSO : RecipeSO
{
    public override void Initialize()
    {
        this.recipeName = "Catfish Night Stew";
        this.description = "Upgrades Boat to level 3:\n" + GameManager.Instance.PlayerEquipmentRegistry.boatSO.detailsPerLevel[2];
        this.ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpMeatSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpToothSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.troutMeatSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.shinyFinSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.shadowingEyeSO, quantity: 3)
        };
        this.hasAlreadyBeenUsed = false;
        this.isFinalRecipe = false;
        this.upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.boatSO;
        this.upgradesToLevel = 3;
    }
}
