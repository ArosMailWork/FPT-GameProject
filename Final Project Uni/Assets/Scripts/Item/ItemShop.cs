using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
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
    [FoldoutGroup("Setup")]
    [SerializeField] private List<GameObject> allShopItems;
    [FoldoutGroup("Setup")]
    public int ItemSellAmount = 3;
    [FoldoutGroup("Setup")]
    [SerializeField] private List<Transform> ItemSellingPlacement;
    [FoldoutGroup("Setup")]
    [SerializeField] private List<TextMeshProUGUI> ItemSellingPrice;
    [FoldoutGroup("Setup")]
    [Range(0,1)] public float SkillPercent = 0.4f;

    [FoldoutGroup("Debug")]
    [ReadOnly] public List<ItemSelling> AvailableShopPool;
    [ReadOnly] public List<GameObject> SellingItems;
    
    private PlayerData _playerData;
    
    private void Start()
    {
        _playerData = PlayerController.Instance._playerData;
        CreatePool();
    }
    
    
    [Button]
    public void CreatePool()
    {
        AvailableShopPool = GenerateItemShopPool();
        SetSellingItems();
    }

    [Button]
    public void SetSellingItems()
    {
        SellingItems.Clear();  // Clear previous items

        // Separate skill and non-skill items
        var nonSkillItems = AvailableShopPool.Where(item => !item.isSkill).ToList();
        var skillItems = AvailableShopPool.Where(item => item.isSkill).ToList();

        for (int i = 0; i < ItemSellAmount; i++)
        {
            // 25% chance to add a skill item, otherwise add a non-skill item
            if (skillItems.Count > 0 && Random.Range(0f, 1f) <= SkillPercent)
            {
                int randomSkillIndex = Random.Range(0, skillItems.Count);
                SellingItems.Add(skillItems[randomSkillIndex].item);
            }
            else if (nonSkillItems.Count > 0)
            {
                int randomNonSkillIndex = Random.Range(0, nonSkillItems.Count);
                var selectedItem = nonSkillItems[randomNonSkillIndex];
                SellingItems.Add(selectedItem.item);

                // Adjust cost for duplicates
                var interactableItem = selectedItem.item.GetComponentInChildren<InteractableItem>();
                int duplicateCount = SellingItems.Count(x => x == selectedItem.item);
                if (duplicateCount > 1 && interactableItem != null)
                {
                    interactableItem.Cost = Mathf.RoundToInt(interactableItem.Cost * Mathf.Pow(1.5f, duplicateCount - 1));
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

        SetItemToPlace();
    }

    void SetItemToPlace()
    {
        // Spawn each item in the SellingItems list at the respective position in ItemSellingPlacement
        for (int i = 0; i < SellingItems.Count && i < ItemSellingPlacement.Count; i++)
        {
            var item = SellingItems[i];
            var placement = ItemSellingPlacement[i];

            // Instantiate or move the item to the placement position
            var spawnedItem = Instantiate(item, placement.position, placement.rotation);
        
            var interactableItem = spawnedItem.GetComponentInChildren<InteractableItem>();
            if (interactableItem != null)
                interactableItem.isItemShop = true;

            if(ItemSellingPrice[i] != null)
                ItemSellingPrice[i].text = interactableItem.Cost.ToString();
        }
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
            
            //Debug.Log(item.isSkillBuff);
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
