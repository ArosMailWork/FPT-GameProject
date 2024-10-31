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

// This will be used for saving/loading data to JSON
[System.Serializable]
public class PlayerDataSave
{
    public List<SkillUnlock> unlockedSkills = new List<SkillUnlock>();
    public List<InventoryItem> Inventory = new List<InventoryItem>();
}

public class PlayerData : MonoBehaviour
{
    public SkillDatabase skillDatabase; 
    public List<SkillUnlock> unlockedSkills = new List<SkillUnlock>();
    public List<InventoryItem> Inventory = new List<InventoryItem>();

    private void Start()
    {
        LoadPlayerData();
    }
    
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
            Debug.LogWarning($"Skill ID {skillID} not found in the skill database.");
        }
    }


    [Button("Save")]
    public void SavePlayerData()
    {
        // Create a serializable save object
        var saveData = new PlayerDataSave
        {
            unlockedSkills = new List<SkillUnlock>(unlockedSkills),
            Inventory = new List<InventoryItem>(Inventory)
        };
        
        PlayerDataCRUD.SavePlayerData(saveData);
    }
    [Button("Load")]
    public void LoadPlayerData()
    {
        PlayerDataSave saveData = PlayerDataCRUD.LoadPlayerData();
        
        unlockedSkills.Clear();
        Inventory.Clear();
        if (saveData != null)
        {
            unlockedSkills = new List<SkillUnlock>(saveData.unlockedSkills);
            Inventory = new List<InventoryItem>(saveData.Inventory);
            Debug.Log("Player data loaded.");
        }
        else
        {
            Debug.LogWarning("No save data found. Initializing new data.");
            InitializeSkillsFromDatabase();
        }
    }

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
