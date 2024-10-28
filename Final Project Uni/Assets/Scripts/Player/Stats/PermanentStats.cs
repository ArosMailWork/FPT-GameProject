using System.IO;
using UnityEngine;

namespace PA
{
    public static class PermanentStats
    {
        //private static string SavePath =>Path.Combine(Application.persistentDataPath, "player_stats.json");
        private static string SavePath => Path.Combine(Application.dataPath, "player_stats.json");
        public static float HP { get; private set; } = 1f;
        public static float Speed { get; private set; } = 1f;
        public static float Damage { get; private set; } = 1f;

        public static void LoadStats()
        {
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
                PermanentStatsData loadedData = JsonUtility.FromJson<PermanentStatsData>(json);

                // Apply loaded stats to static properties
                HP = loadedData.HP;
                Speed = loadedData.Speed;
                Damage = loadedData.Damage;
            }
        }

        public static void SaveStats()
        {
            var data = new PermanentStatsData
            {
                HP = HP,
                Speed = Speed,
                Damage = Damage,
            };
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(SavePath, json);
        }

        public static void UpdateStat(string statName, float value)
        {
            switch (statName.ToLower())
            {
                case "hp":
                    HP = value;
                    break;
                case "speed":
                    Speed = value;
                    break;
                case "damage":
                    Damage = value;
                    break;
            }
        }

        [System.Serializable]
        private class PermanentStatsData
        {
            public float HP;
            public float Speed;
            public float Damage;
        }
    }
}
