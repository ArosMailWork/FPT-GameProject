using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public struct PlayerSkill
{
    public string Skill_ID;
    public GameObject skillPrefab;
}

[System.Serializable]
public class SkillUnlock
{
    public PlayerSkill Skill;
    public bool isUnlocked;
}

[System.Serializable]
public struct InventoryItem
{
    public string ID;
    public int amount;
}

[System.Serializable]
public class PermaStatsData
{
    public int knowledgeLevel;
    public int Soul;
    public int HPUpgradesData;
    public int SpeedUpgradesData;
    public int DamageUpgradesData;
    public int StaminaUpgradesData;
    public int StaminaRegenUpgradesData;
}

// This will be used for saving/loading data to JSON
[System.Serializable]
public class PlayerDataSave
{
    public List<SkillUnlock> SkillList = new List<SkillUnlock>();
    public List<InventoryItem> Inventory = new List<InventoryItem>();
}

public class PlayerData : MonoBehaviour
{
    public SkillDatabase skillDatabase; 
    public int Gold, SoulCollected;
    public List<SkillUnlock> unlockedSkills = new List<SkillUnlock>();
    public List<InventoryItem> Inventory = new List<InventoryItem>();

    private void Start()
    {
        LoadPlayerData();
    }
    
    [Button]
    public void UnlockSkill(string skillID)
    {
        // Check if the skill exists in the player's unlockedSkills list
        var skill = unlockedSkills.Find(s => s.Skill.Skill_ID == skillID);
        if (skill != null)
        {
            skill.isUnlocked = true; // Unlock the skill
            Debug.Log($"Skill {skillID} unlocked.");
        }
        else
        {
            Debug.Log($"Skill ID {skillID} not found in the skill database.");
        }
    }

    public void AdjustCurrency(float InputGold = 0, float InputSoul = 0)
    {
        Gold += Mathf.RoundToInt(InputGold);
        SoulCollected += Mathf.RoundToInt(InputSoul);
    }

    
    //Call this command whenever player pass a floor
    [Button("Save")]
    public void SavePlayerData()
    {
        var saveData = new PlayerDataSave
        {
            SkillList = new List<SkillUnlock>(unlockedSkills),
            Inventory = new List<InventoryItem>(Inventory)
        };
        
        PlayerDataCRUD.SavePlayerData(saveData);
        
        // Load existing Soul value
        PermaStatsData permaStats = PlayerDataCRUD.LoadPermanentStats();
        permaStats.Soul += SoulCollected;
    
        // Save the updated Soul value
        PlayerDataCRUD.SavePermanentStats(permaStats);
    }
    
    [Button("Load")]
    public void LoadPlayerData()
    {
        PlayerDataSave saveData = PlayerDataCRUD.LoadPlayerData();
        
        unlockedSkills.Clear();
        Inventory.Clear();
        if (saveData != null)
        {
            unlockedSkills = new List<SkillUnlock>(saveData.SkillList);
            Inventory = new List<InventoryItem>(saveData.Inventory);
            Debug.Log("Player data loaded.");
        }
        else
        {
            Debug.LogWarning("No save data found. Initializing new data.");
            InitializeSkillsFromDatabase();
        }
    }

    //Ults
    private void InitializeSkillsFromDatabase()
    {
        foreach (var skill in skillDatabase.allSkills)
        {
            unlockedSkills.Add(new SkillUnlock { Skill = skill, isUnlocked = false });
        }
    }
    public bool IsSkillUnlocked(string skillID)
    {
        var skill = unlockedSkills.Find(s => s.Skill.Skill_ID == skillID);
        return skill != null && skill.isUnlocked;
    }
}
