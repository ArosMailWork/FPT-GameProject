using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CraftingRecipe
{
    public string RecipeID;
    public GameObject output;
    public List<InventoryItem> input;
    public bool defaultUnlocked;
}

[System.Serializable]
public class RecipeUnlock
{
    public CraftingRecipe Recipe;
    public bool isUnlocked;
}

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "ScriptableObjects/RecipeDatabase", order = 1)]
public class RecipeDatabase : ScriptableObject
{
    public List<CraftingRecipe> AllRecipes;
}