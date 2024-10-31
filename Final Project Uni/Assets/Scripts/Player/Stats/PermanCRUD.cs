using System.IO;
using UnityEngine;


[System.Serializable]
public class PermaStatsData
{
    public int knowledgeLevel;
    public int Gold;
    public int HPUpgradesData;
    public int SpeedUpgradesData;
    public int DamageUpgradesData;
    public int StaminaUpgradesData;
    public int StaminaRegenUpgradesData;
}

public static class PermanCRUD
{
    private static readonly string SavePath = Path.Combine(
        Application.dataPath,
        "player_perma_stats.json"
    );

    public static void SavePermanentStats(PermaStatsData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Perma stats saved at {SavePath}");
    }

    public static PermaStatsData LoadPermanentStats()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PermaStatsData>(json);
        }
        return new PermaStatsData();
    }
}
