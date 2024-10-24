using System.IO;
using UnityEngine;

public static class PermanCRUD
{
    private static readonly string SavePath = Path.Combine(
        Application.dataPath,
        "player_stats.json"
    );
    public static void SavePermanentStats(PermanentStatsData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Stats saved permanently at {SavePath}");
    }

    public static PermanentStatsData LoadPermanentStats()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PermanentStatsData>(json);
        }
        return new PermanentStatsData();
    }
}