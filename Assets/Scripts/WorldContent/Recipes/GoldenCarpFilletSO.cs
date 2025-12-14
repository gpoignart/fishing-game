using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/GoldenCarpFillet")]
public class GoldenCarpFilletSO : RecipeSO
{
    public override void Initialize()
    {
        recipeName = "Golden Carp Fillet";
        description = "Upgrades Fishing Rod to level 3.";
        ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpMeatSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpToothSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.glimmeringScaleSO, quantity: 3)
        };
        hasAlreadyBeenUsed = false;
        isFinalRecipe = false;
        upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.fishingRodSO;
        upgradesToLevel = 3;
    }
}
