    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

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
        public List<RecipeUnlock> RecipeList = new List<RecipeUnlock>();
        public List<InventoryItem> Inventory = new List<InventoryItem>();
    }

    public class PlayerData : MonoBehaviour
    {
        [FoldoutGroup("Setup")]
        public ItemDatabase itemDatabase;
        [FoldoutGroup("Setup")]
        public RecipeDatabase recipeDatabase;
        [FoldoutGroup("Setup")]
        public SkillDatabase skillDatabase;

        [FoldoutGroup("Setup")] 
        [CanBeNull] public TextMeshProUGUI GoldText;
        
        [FoldoutGroup("Inventory")]
        public int Gold, SoulCollected;
        [FoldoutGroup("Inventory")]
        public List<SkillUnlock> unlockedSkills = new List<SkillUnlock>();
        [FoldoutGroup("Inventory")]
        public List<InventoryItem> Inventory = new List<InventoryItem>();
        [FoldoutGroup("Inventory")]
        public List<RecipeUnlock> unlockedRecipes = new List<RecipeUnlock>();

        private void Start()
        {
            LoadPlayerData();
        }

        #region Modify
        
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
        [Button]
        public void UnlockRecipe(string recipeID)
        {
            var recipe = unlockedRecipes.Find(r => r.Recipe.RecipeID == recipeID);
            if (recipe != null)
            {
                recipe.isUnlocked = true;
                Debug.Log($"Recipe {recipeID} unlocked.");
            }
            else
            {
                Debug.Log($"Recipe ID {recipeID} not found in the recipe database.");
            }
        }
        
        public void AddCurrency(float InputGold = 0, float InputSoul = 0)
        {
            Gold += Mathf.RoundToInt(InputGold);
            SoulCollected += Mathf.RoundToInt(InputSoul);

            if (GoldText != null) GoldText.text = Gold.ToString();
        }
        
        [Button]
        public void AddItem(string itemID, int amount, bool minus = false)
        {
            // Guard clause: Check if itemID is valid
            var itemData = itemDatabase.allItems.Find(i => i.ID == itemID);
            if (itemData == null)
            {
                Debug.Log($"Item with ID {itemID} not found in the item database.");
                return;
            }

            // Guard clause: Check if the amount to add is non-zero
            int finalAmount = minus ? -amount : amount;
            if (finalAmount == 0)
            {
                Debug.Log("Attempted to add zero items.");
                return;
            }

            // Find if the item already exists in the inventory
            var inventoryItem = Inventory.Find(i => i.item.ID == itemID);

            if (inventoryItem != null)
            {
                // Adjust the existing item's amount
                inventoryItem.amount += finalAmount;

                // Remove item if amount falls to zero or below
                if (inventoryItem.amount <= 0)
                {
                    Inventory.Remove(inventoryItem);
                    Debug.Log($"Removed {itemID} from inventory as the amount reached zero.");
                }
            }
            else if (finalAmount > 0)
            {
                // Add a new item entry if it doesn't exist and the amount is positive
                Inventory.Add(new InventoryItem { item = itemData, amount = finalAmount });
                Debug.Log($"Added {amount} of {itemID} to inventory.");
            }
            else
            {
                Debug.Log($"Cannot add a negative or zero amount of {itemID} to inventory.");
            }
        }
        
        #endregion

        #region Save Load

        // Call this command whenever player passes a floor
        [Button("Confirm Reward")]
        public void ConfirmReward()
        {
            // Create a PlayerDataSave object to save isUnlocked states and inventory
            var saveData = new PlayerDataSave();

            // Save skill states
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
            
            // Save recipe states
            foreach (var recipe in recipeDatabase.AllRecipes)
            {
                var unlockedRecipe = unlockedRecipes.Find(r => r.Recipe.RecipeID == recipe.output.name);
                saveData.RecipeList.Add(new RecipeUnlock
                {
                    Recipe = recipe,
                    isUnlocked = unlockedRecipe != null ? unlockedRecipe.isUnlocked : recipe.defaultUnlocked
                });
            }

            // Save inventory items
            foreach (var inventoryItem in Inventory)
            {
                saveData.Inventory.Add(new InventoryItem
                {
                    item = inventoryItem.item,
                    amount = inventoryItem.amount
                });
            }
            
            // Save skill and inventory data
            PlayerDataCRUD.SavePlayerData(saveData);

            // Load existing Soul value
            PermaStatsData permaStats = PlayerDataCRUD.LoadPermanentStats();
            permaStats.Soul += SoulCollected;

            // Save the updated Soul value
            PlayerDataCRUD.SavePermanentStats(permaStats);
        }

        // Call this when you intend to do something with shopping, etc.
        [Button("Load")]
        public void LoadPlayerData()
        {
            PlayerDataSave saveData = PlayerDataCRUD.LoadPlayerData();

            unlockedSkills.Clear();
            unlockedRecipes.Clear();
            Inventory.Clear();

            if (saveData != null)
            {
                // Load inventory data
                foreach (var savedItem in saveData.Inventory)
                {
                    var itemData = itemDatabase.allItems.Find(i => i.ID == savedItem.item?.ID);
                    if (itemData != null)
                    {
                        Inventory.Add(new InventoryItem
                        {
                            item = itemData,
                            amount = savedItem.amount
                        });
                    }
                    else
                    {
                        Debug.Log("empty inventory");
                    }
                }

                // Load skill data
                foreach (var skill in skillDatabase.allSkills)
                {
                    var savedSkill = saveData.SkillList.Find(s => s.Skill.Skill_ID == skill.Skill_ID);
                    unlockedSkills.Add(new SkillUnlock
                    {
                        Skill = skill,
                        isUnlocked = savedSkill != null ? savedSkill.isUnlocked : skill.defaultUnlocked
                    });
                }

                // Load recipe data
                if (recipeDatabase == null || recipeDatabase.AllRecipes == null)
                {
                    Debug.Log("Found no Recipe in DB");
                    return;
                }

                foreach (var recipe in recipeDatabase.AllRecipes)
                {
                    if (recipe == null || recipe.output == null)
                    {
                        Debug.Log("Invalid recipe: " + recipe.RecipeID);
                        continue;
                    }

                    var savedRecipe = saveData.RecipeList.Find(r => r.Recipe != null && r.Recipe.output != null && r.Recipe.output.name == recipe.output.name);

                    unlockedRecipes.Add(new RecipeUnlock
                    {
                        Recipe = recipe,
                        isUnlocked = savedRecipe != null ? savedRecipe.isUnlocked : recipe.defaultUnlocked
                    });
                }

                //Debug.Log("Player data loaded and synchronized with skill and recipe database.");
            }
            else
            {
                Debug.LogWarning("No save data found. Initializing new data from database.");
                InitializeSkillsFromDatabase();
                InitializeRecipesFromDatabase();
            }
        }


        #endregion


        #region Calculate Ults

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
        private void InitializeRecipesFromDatabase()
        {
            foreach (var recipe in recipeDatabase.AllRecipes)
            {
                unlockedRecipes.Add(new RecipeUnlock
                {
                    Recipe = recipe,
                    isUnlocked = recipe.defaultUnlocked
                });
            }
        }

        
        public bool IsSkillUnlocked(string skillID)
        {
            var skill = unlockedSkills.Find(s => s.Skill.Skill_ID == skillID);
            return skill != null && skill.isUnlocked;
        }
        
        public bool IsRecipeUnlocked(string recipeID)
        {
            var recipe = unlockedRecipes.Find(r => r.Recipe.RecipeID == recipeID);
            return recipe != null && recipe.isUnlocked;
        }
        #endregion
    }
