using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public struct ItemDrop
{
    public GameObject itemPrefab;
    [Range(0, 100)] public float dropRate; // Percentage scale (0 - 100%)
}

public class ItemDropRate : MonoBehaviour
{
    public bool DropOneItemOnly;
    public List<ItemDrop> itemDrops;
    
    public int MinSoul = 0, MaxSoul;
    public int MinGold = 0, MaxGold;
    
    // Customizable offset
    public Vector3 FixedOffset = new Vector3(0, 2f, 0); 
    public float range = 2f;

    private int Gold, Soul;

    [Button]
    public void CalculateDrops()
    {
        if (DropOneItemOnly)
            DropOneItem();
        else
            DropMultipleItems();
        
        DropCurrency();
    }

    private void DropOneItem()
    {
        float totalProbability = 0f;
        float randomValue = Random.Range(0f, 100f);

        foreach (var itemDrop in itemDrops)
        {
            totalProbability += itemDrop.dropRate;

            if (randomValue <= totalProbability)
            {
                Drop(itemDrop.itemPrefab);
                return; // Drop only one item, then exit
            }
        }
    }
    private void DropMultipleItems()
    {
        foreach (var itemDrop in itemDrops)
        {
            float randomValue = Random.Range(0f, 100f);
            if (randomValue <= itemDrop.dropRate)
            {
                Drop(itemDrop.itemPrefab);
            }
        }
    }

    private void DropCurrency()
    {
        Gold = Random.Range(MinGold, MaxGold);
        Soul = Random.Range(MinSoul, MaxSoul);
        
        //temporary add directly, will add item collide to add money later
        PlayerController.Instance._playerData.AddCurrency(Gold, Soul);
    }

    private void Drop(GameObject item)
    {
        if (item != null)
        {
            // Generate a random offset with y set to 1
            Vector3 randomOffset = new Vector3(
                Random.Range(-range, range), 
                1f, 
                Random.Range(-range, range)
            );

            // Instantiate item with the combined offset
            Instantiate(item, transform.position + randomOffset + FixedOffset, Quaternion.identity);
            Debug.Log($"Dropped: {item.name}");
        }
    }
}