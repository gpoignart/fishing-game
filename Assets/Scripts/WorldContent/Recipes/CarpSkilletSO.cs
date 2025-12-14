using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/CarpSkillet")]
public class CarpSkilletSO : RecipeSO
{
    public override void Initialize()
    {
        this.recipeName = "Carp Skillet";
        this.description = "Upgrades Fishing Rod to level 2.";
        this.ingredients = new RecipeIngredient[]
        {
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpMeatSO, quantity: 5),
            new RecipeIngredient(ingredientSO: GameManager.Instance.IngredientRegistry.carpToothSO, quantity: 5)
        };
        this.hasAlreadyBeenUsed = false;
        this.isFinalRecipe = false;
        this.upgradesEquipment = GameManager.Instance.PlayerEquipmentRegistry.fishingRodSO;
        this.upgradesToLevel = 2;
    }
}