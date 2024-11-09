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

    [FoldoutGroup("Debug")] 
    [ReadOnly] public List<ItemSelling> nonSkillItems, skillItems;
    [FoldoutGroup("Debug")] 
    [ReadOnly] public List<GameObject> SellingItems;
    
    private PlayerData _playerData;
    
    private void Start()
    {
        _playerData = PlayerController.Instance._playerData;
        Invoke(nameof(CreatePool), 0.5f);
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
        nonSkillItems = AvailableShopPool.Where(item => !item.isSkill).ToList();
        skillItems = AvailableShopPool.Where(item => item.isSkill).ToList();

        for (int i = 0; i < ItemSellAmount; i++)
        {
            float randomChance = Random.Range(0f, 1f);
            Debug.Log((randomChance <= SkillPercent) + " | " + skillItems.Count);
            // 25% chance to add a skill item, otherwise add a non-skill item
            if (skillItems.Count > 0 && randomChance <= SkillPercent)
            {
                int randomSkillIndex = Random.Range(0, skillItems.Count);
                SellingItems.Add(skillItems[randomSkillIndex].item);
                Debug.Log("add " + skillItems[randomSkillIndex].item);
            }
            else if (nonSkillItems.Count > 0)
            {
                int randomNonSkillIndex = Random.Range(0, nonSkillItems.Count);
                var selectedItem = nonSkillItems[randomNonSkillIndex];
                SellingItems.Add(selectedItem.item);

                // Adjust cost for duplicates by increasing by 50% of the original cost for each duplicate
                var interactableItem = selectedItem.item.GetComponentInChildren<InteractableItem>();
                if (interactableItem != null)
                {
                    int duplicateCount = SellingItems.Count(x => x == selectedItem.item) - 1; // Count duplicates after the first instance
                    interactableItem.LastCost = interactableItem.Cost + (int)(interactableItem.Cost * 0.5f * duplicateCount);
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

            ItemSelling itemSelling = new ItemSelling { item = itemObj, isSkill = item.isSkillBuff};
            
            if (item.isSkillBuff)
            {
                string skillID = item.Skill.GetComponent<ISkill>().Name;
                //Debug.Log(skillID);

                if (IsSkillUnlockedForItem(skillID))
                    pool.Add(itemSelling);
            }
            else
            {
                pool.Add(itemSelling);
            }
        }

        return pool;
    }

    [Button]
    public void Print(string SkillID)
    {
        Debug.Log( "work ? " + IsSkillUnlockedForItem(SkillID));
    }
    
    private bool IsSkillUnlockedForItem(string skillID)
    {
        // Check in PlayerData if the skill is unlocked
        return _playerData.IsSkillUnlocked(skillID);
    }
}
