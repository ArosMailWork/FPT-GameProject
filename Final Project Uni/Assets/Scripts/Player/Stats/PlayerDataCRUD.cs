using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerDataCRUD
{
    private static readonly string SavePath = Path.Combine(
        Application.persistentDataPath,
        "player_data.json"
    );

    public static void SavePlayerData(PlayerDataSave data)
    {
        // Serialize the PlayerDataSave object to JSON
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Player data saved at {SavePath}");
    }

    public static PlayerDataSave LoadPlayerData()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PlayerDataSave>(json);
        }
        return null;
    }
}