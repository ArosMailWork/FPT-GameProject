using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeUI : MonoBehaviour
{
    public TMP_Text RecipeNameText;
    public Image RecipeIcon;
    public GameObject MissingMatSprite; // Indicator if materials are missing
    public Button CraftButton;
    private string recipeID;
    private CraftingTable craftingTable;

    /// <summary>
    /// Initializes the Recipe UI element with recipe data.
    /// </summary>
    public void SetRecipeUI(CraftingRecipe recipe, bool canCraft, CraftingTable table)
    {
        RecipeNameText.text = recipe.RecipeName;
        RecipeIcon.sprite = recipe.output.GetComponent<SpriteRenderer>().sprite; // Assuming output has a SpriteRenderer
        recipeID = recipe.RecipeID;
        craftingTable = table;

        // Show or hide the missing material indicator
        MissingMatSprite.SetActive(!canCraft);

        // Set up the Craft button
        CraftButton.onClick.RemoveAllListeners();
        CraftButton.onClick.AddListener(() => craftingTable.Crafting(recipeID));
    }
}