using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public struct ItemDrop
{
    public GameObject itemPrefab;
    [Range(0, 100)]
    public float dropRate; // Percentage scale (0 - 100%)
}

public class ItemDropRate : MonoBehaviour
{
    public bool DropOneItemOnly;
    public List<ItemDrop> itemDrops;

    [Button]
    public void CalculateDrops()
    {
        if (DropOneItemOnly)
            DropOneItem();
        else
            DropMultipleItems();
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

    private void Drop(GameObject item)
    {
        if (item != null)
        {
            Instantiate(item, transform.position + (Vector3.up * 2f), Quaternion.identity);
            Debug.Log($"Dropped: {item.name}");
        }
    }
}