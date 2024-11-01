using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public struct PlayerSkill
{
    public string Skill_ID;
    public GameObject skillPrefab;
    public bool defaultUnlocked;
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
    [Button("Confirm Reward")]
    public void ConfirmReward()
    {
        // Create a PlayerDataSave object to save isUnlocked states
        var saveData = new PlayerDataSave();

        foreach (var skill in skillDatabase.allSkills)
        {
            // Find if this skill is in the current unlockedSkills list
            var unlockedSkill = unlockedSkills.Find(s => s.Skill.Skill_ID == skill.Skill_ID);
        
            saveData.SkillList.Add(new SkillUnlock
            {
                Skill = skill,
                // Use isUnlocked from unlockedSkills if available, or defaultUnlocked from database
                isUnlocked = unlockedSkill != null ? unlockedSkill.isUnlocked : skill.defaultUnlocked 
            });
        }
        
        PlayerDataCRUD.SavePlayerData(saveData);
        
        
        // Load existing Soul value
        PermaStatsData permaStats = PlayerDataCRUD.LoadPermanentStats();
        permaStats.Soul += SoulCollected;
    
        // Save the updated Soul value
        PlayerDataCRUD.SavePermanentStats(permaStats);
    }
    
    //Call this when u intend to do sth with shopping, etc
    [Button("Load")]
    public void LoadPlayerData()
    {
        // Load saved data
        PlayerDataSave saveData = PlayerDataCRUD.LoadPlayerData();

        // Clear current lists
        unlockedSkills.Clear();
        Inventory.Clear();

        // If there’s saved data, synchronize it with skillDatabase
        if (saveData != null)
        {
            // Load inventory data
            Inventory = new List<InventoryItem>(saveData.Inventory);

            // Sync with skillDatabase and override properties from database
            foreach (var skill in skillDatabase.allSkills)
            {
                // Find the saved skill to check its unlocked state
                var savedSkill = saveData.SkillList.Find(s => s.Skill.Skill_ID == skill.Skill_ID);

                unlockedSkills.Add(new SkillUnlock
                {
                    Skill = skill, // Take all properties from the database
                    isUnlocked = savedSkill != null ? savedSkill.isUnlocked : skill.defaultUnlocked // Use saved isUnlocked or defaultUnlocked from the database
                });
            }

            Debug.Log("Player data loaded and synchronized with skill database.");
        }
        else
        {
            Debug.LogWarning("No save data found. Initializing new data from database.");
            InitializeSkillsFromDatabase();
        }
    }

    //First Time
    private void InitializeSkillsFromDatabase()
    {
        foreach (var skill in skillDatabase.allSkills)
        {
            // Set isUnlocked to true if the skill's defaultUnlocked property is true
            unlockedSkills.Add(new SkillUnlock 
            { 
                Skill = skill, 
                isUnlocked = skill.defaultUnlocked 
            });
        }
    }
    public bool IsSkillUnlocked(string skillID)
    {
        var skill = unlockedSkills.Find(s => s.Skill.Skill_ID == skillID);
        return skill != null && skill.isUnlocked;
    }
}
