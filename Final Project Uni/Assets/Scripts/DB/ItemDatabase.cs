using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InventoryItem
{
    public gameItem item;
    public int amount;
}

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 1)]
public class ItemDatabase : ScriptableObject
{
    public List<gameItem> allItems; // List of all item in the game
}