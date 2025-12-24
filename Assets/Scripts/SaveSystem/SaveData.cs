using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public string currentTimeOfDayName;
    public int daysCount;
    public int nightsCount;
    public bool isFirstDay;
    public bool isFirstNight;
    public bool isFishingTutorialEnabled;
    public bool isMapSelectionExplanationEnabled;
    public bool isRecipeBookUnlocked;
    public bool isRecipeBookOpened;
    public bool isGameEnded;
    public List<IngredientSaveData> ingredients = new();
    public List<PlayerEquipmentSaveData> playerEquipments = new();
    public List<RecipeSaveData> recipes = new();
}

[System.Serializable]
public class IngredientSaveData
{
    public string ingredientName;
    public int playerQuantityPossessed;
}

[System.Serializable]
public class PlayerEquipmentSaveData
{
    public string playerEquipmentName;
    public int level;
}

[System.Serializable]
public class RecipeSaveData
{
    public string recipeName;
    public bool hasAlreadyBeenUsed;
}
