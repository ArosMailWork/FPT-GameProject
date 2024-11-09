using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct ItemSelling
{
    public GameObject item;
    public bool isSkill;
}

public class ItemShop : MonoBehaviour
{
    // Reference to PlayerData to access unlocked skills and stats
    private PlayerData _playerData;
    [SerializeField] private List<GameObject> allShopItems;

    [FoldoutGroup("Debug")]
    [ReadOnly] public List<ItemSelling> AvailableShopPool;
    
    [ReadOnly] public List<GameObject> SellingItems;
    
    private void Start()
    {
        _playerData = PlayerController.Instance._playerData;
    }

    [Button]
    public void PopulateSellingItems()
    {
        SellingItems.Clear();  // Clear previous items

        // Separate skill and non-skill items
        var nonSkillItems = AvailableShopPool.Where(item => !item.isSkill).ToList();
        var skillItems = AvailableShopPool.Where(item => item.isSkill).ToList();

        for (int i = 0; i < 3; i++)
        {
            // 25% chance to add a skill item, otherwise add a non-skill item
            if (skillItems.Count > 0 && Random.Range(0f, 1f) <= 0.25f)
            {
                int randomSkillIndex = Random.Range(0, skillItems.Count);
                SellingItems.Add(skillItems[randomSkillIndex].item);
            }
            else if (nonSkillItems.Count > 0)
            {
                int randomNonSkillIndex = Random.Range(0, nonSkillItems.Count);
                var selectedItem = nonSkillItems[randomNonSkillIndex];
                SellingItems.Add(selectedItem.item);

                // Check for duplicates and increase cost by 50% for one of the duplicates
                var interactableItem = selectedItem.item.GetComponentInChildren<InteractableItem>();
                if (SellingItems.Count(x => x == selectedItem.item) > 1)
                {
                    interactableItem.Cost = Mathf.RoundToInt(interactableItem.Cost * 1.5f);
                }
            }
            
            // Enable isItemShop for each selected item
            foreach (var item in SellingItems)
            {
                var interactableItem = item.GetComponentInChildren<InteractableItem>();
                if (interactableItem != null)
                    interactableItem.isItemShop = true;
            }
        }
    }


    [Button]
    public void CreatePool()
    {
        AvailableShopPool = GenerateItemShopPool();
    }
    
    //Calculate
    private List<ItemSelling> GenerateItemShopPool()
    {
        List<ItemSelling> pool = new List<ItemSelling>();

        foreach (var itemObj in allShopItems)
        {
            
            var item = itemObj.GetComponentInChildren<Item>();
            item.Awake();

            ItemSelling itemSelling = new ItemSelling { item = itemObj };
            
            Debug.Log(item.isSkillBuff);
            if (item.isSkillBuff)
            {
                string skillID = item.Skill.GetComponent<ISkill>()?.Name;

                if (IsSkillUnlockedForItem(skillID))
                {
                    itemSelling.isSkill = true;  // Set isSkill if skill is unlocked
                    pool.Add(itemSelling);
                }
            }
            else
            {
                itemSelling.isSkill = false;  // Set isSkill to false for non-skill items
                pool.Add(itemSelling);
            }
        }

        return pool;
    }

    private bool IsSkillUnlockedForItem(string skillID)
    {
        // Check in PlayerData if the skill is unlocked
        return _playerData.IsSkillUnlocked(skillID);
    }
}
