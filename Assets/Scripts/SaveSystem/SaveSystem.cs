using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SaveFileName = "save.json";

    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, SaveFileName);

    // Check if a save exists
    public static bool HasSave()
    {
        return File.Exists(SavePath);
    }

    // Save
    public static void Save(SaveData data)
    {
        Debug.Log("Save path: " + Application.persistentDataPath);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    // Load
    public static SaveData Load()
    {
        if (!HasSave()) { return null; }
        
        Debug.Log("Load called");

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<SaveData>(json);
    }

    // Delete
    public static void DeleteSave()
    {
        if (HasSave()) { File.Delete(SavePath); }
    }
}
