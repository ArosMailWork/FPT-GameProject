using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class CraftingTableUI : MonoBehaviour
{
    public GameObject RecipeUIPrefab;
    public Transform RecipeListContent;
    [HideInInspector] public CraftingTable craftingTable;

    private PlayerData playerData;

    [Button("Update Recipe")]
    public void OnEnable()
    {
        UpdateRecipeList();
    }

    /// Updates the list of recipes displayed in the crafting UI.
    public void UpdateRecipeList()
    {
        playerData = PlayerController.Instance._playerData;
        
        // Clear existing UI elements
        foreach (Transform child in RecipeListContent)
        {
            Destroy(child.gameObject);
        }

        // Get recipes and display each in the UI
        foreach (var recipe in playerData.unlockedRecipes)
        {
            bool canCraft = CheckIfCanCraft(recipe.Recipe);
            var recipeUIObj = Instantiate(RecipeUIPrefab, RecipeListContent);
            RecipeUI recipeUI = recipeUIObj.GetComponent<RecipeUI>();

            // Initialize the RecipeUI element
            recipeUI.SetRecipeUI(recipe.Recipe, canCraft, craftingTable, playerData.IsRecipeUnlocked(recipe.Recipe.RecipeID));
        }
    }

    /// Checks if the player has enough materials for a given recipe.
    private bool CheckIfCanCraft(CraftingRecipe recipe)
    {
        foreach (var inputItem in recipe.input)
        {
            var inventoryItem = playerData.Inventory.Find(i => i.item.ID == inputItem.item.ID);
            if (inventoryItem == null || inventoryItem.amount < inputItem.amount)
            {
                return false;
            }
        }
        return true;
    }
}