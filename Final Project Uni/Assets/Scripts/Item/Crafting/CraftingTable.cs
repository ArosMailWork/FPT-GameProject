using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    [CanBeNull] public Transform CraftedItemPlacement;
    public CraftingTableUI craftingTableUI;

    PlayerData _playerData;

    private void Awake()
    {
        if(craftingTableUI != null)
        craftingTableUI.craftingTable = this;
    }

    private void Start()
    {
        _playerData = PlayerController.Instance._playerData;
    }

    [Button]
    public void Crafting(string recipeID)
    {
        if(_playerData == null) _playerData = PlayerController.Instance._playerData;
        
        // Find the recipe in the player's unlocked recipes
        var recipe = _playerData.unlockedRecipes.Find(r => r.Recipe.RecipeID == recipeID && r.isUnlocked);
        if (recipe == null)
        {
            Debug.Log("Recipe not found or is locked.");
            return;
        }

        // Dictionary to track missing items and their amounts
        Dictionary<string, int> missingItems = new Dictionary<string, int>();
        
        // Check if the player has the required items and amounts
        foreach (var inputItem in recipe.Recipe.input)
        {
            var inventoryItem = _playerData.Inventory.Find(i => i.item.ID == inputItem.item.ID);

            // If the item is not found or the amount is insufficient
            if (inventoryItem == null || inventoryItem.amount < inputItem.amount)
            {
                int missingAmount = inputItem.amount - (inventoryItem?.amount ?? 0);
                missingItems[inputItem.item.ID] = missingAmount;
            }
        }

        // If there are missing items, show a message with details
        if (missingItems.Count > 0)
        {
            foreach (var missingItem in missingItems)
            {
                Debug.Log($"Missing {missingItem.Key}: need {missingItem.Value} more.");
            }
            return;
        }

        // If all items are available, deduct items from the inventory and spawn the output object
        foreach (var inputItem in recipe.Recipe.input)
        {
            var inventoryItem = _playerData.Inventory.Find(i => i.item.ID == inputItem.item.ID);
            inventoryItem.amount -= inputItem.amount;

            // Remove item if amount reaches zero
            if (inventoryItem.amount <= 0)
                _playerData.Inventory.Remove(inventoryItem);
        }

        Vector3 pos = transform.position;
        if (CraftedItemPlacement != null) pos = CraftedItemPlacement.position;
        // Instantiate the crafted item at the specified location
        Instantiate(recipe.Recipe.output, pos, Quaternion.identity);

        // Save the updated inventory to maintain persistence
        _playerData.SaveClaimReward();
        
        Debug.Log($"Crafted {recipe.Recipe.output.name} successfully!");
    }
}
