using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/CarpSkillet")]
public class CarpSkilletSO : RecipeSO
{
    public override void Initialize()
    {
        this.recipeName = "Carp Skillet";
        this.description = "Upgrades Fishing Rod to level 2:\n" + GameManager.Instance.PlayerEquipmentRegistry.fishingRodSO.detailsPerLevel[1];
        this.ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpMeatSO, quantity: 10),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpToothSO, quantity: 10)
        };
        this.hasAlreadyBeenUsed = false;
        this.isFinalRecipe = false;
        this.upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.fishingRodSO;
        this.upgradesToLevel = 2;
    }
}