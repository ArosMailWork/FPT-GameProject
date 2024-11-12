using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class Ingame_Save : MonoBehaviour
{
    public PlayerStats Stats;
    public Health playerHealth;
    public List<GameObject> skillList = new List<GameObject>();
    public PlayerData runData;
    public bool AllowLoadSave;
    
    [FoldoutGroup("Expedition")]
    public int Floor, World;
    [FoldoutGroup("Expedition")]
    public World currentWorld;
    [FoldoutGroup("Expedition")]
    public Biome currentBiome;

    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "ExpeditionSaveData.json");
        AllowLoadSave = File.Exists(saveFilePath); // Check if save file exists at startup
    }

    [Button]
    public void Save()
    {
        // Collect current data from game objects
        skillList = SkillHolder.Instance.skillList;
        playerHealth = PlayerController.Instance.PlayerHealth;
        Stats = PlayerController.Instance._stats;
        runData = PlayerController.Instance._playerData;

        currentWorld = ExpeditionManager.Instance.currentWorld;
        currentBiome = ExpeditionManager.Instance.currentBiome;
        Floor = ExpeditionManager.Instance.currentFloorNumber;
        World = ExpeditionManager.Instance.currentWorldNumber;
        

        // Serialize to JSON
        string jsonData = JsonUtility.ToJson(this, true);
        File.WriteAllText(saveFilePath, jsonData);
        AllowLoadSave = true;
        
        Debug.Log("Game saved successfully!");
    }

    [Button]
    public void Load()
    {
        if (!AllowLoadSave)
        {
            Debug.LogWarning("No save data available to load.");
            return;
        }

        // Read JSON file
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            JsonUtility.FromJsonOverwrite(jsonData, this);

            // Apply loaded data to game objects
            PlayerController.Instance.PlayerHealth = playerHealth;
            PlayerController.Instance._stats = Stats;
            PlayerController.Instance._playerData = runData;

            SkillHolder.Instance.skillList.Clear();
            foreach (var skill in skillList)
            {
                SkillHolder.Instance.AddSkill(skill);
            }
            
            //Expedition Load
            ExpeditionManager.Instance.currentWorld = currentWorld;
            ExpeditionManager.Instance.currentBiome = currentBiome;
            ExpeditionManager.Instance.currentFloorNumber = Floor;
            ExpeditionManager.Instance.currentWorldNumber = World;
            

            Debug.Log("Game loaded successfully!");
        }
        else
        {
            Debug.LogWarning("Save file not found!");
        }
    }

    public void DestroySave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            AllowLoadSave = false;
            Debug.Log("Save data deleted successfully!");
        }
        else
        {
            Debug.LogWarning("No save file to delete.");
        }
    }
}
