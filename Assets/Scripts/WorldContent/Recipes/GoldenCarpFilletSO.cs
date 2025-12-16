using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/GoldenCarpFillet")]
public class GoldenCarpFilletSO : RecipeSO
{
    public override void Initialize()
    {
        this.recipeName = "Golden Carp Fillet";
        this.description = "Upgrades Fishing Rod to level 3:\n" + GameManager.Instance.PlayerEquipmentRegistry.fishingRodSO.detailsPerLevel[2];
        this.ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpMeatSO, quantity: 10),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpToothSO, quantity: 10),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.glimmeringScaleSO, quantity: 5)
        };
        this.hasAlreadyBeenUsed = false;
        this.isFinalRecipe = false;
        this.upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.fishingRodSO;
        this.upgradesToLevel = 3;
    }
}
