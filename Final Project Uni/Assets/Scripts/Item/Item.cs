using System;
using JetBrains.Annotations;
using PA;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public enum BuffType
{
    Health, Speed, Damage
}

public class Item : MonoBehaviour
{
    [FoldoutGroup("Base")]
    [CanBeNull] public Image itemSprite;
    
    [FoldoutGroup("Buff")]
    public BuffType type;
    [FoldoutGroup("Buff")]
    public int amount;
    
    [FoldoutGroup("Skill")]
    [CanBeNull] public bool takeSkillIcon = true;
    [FoldoutGroup("Skill")]
    [CanBeNull] public GameObject Skill;

    [FoldoutGroup("Material")] 
    public string itemID;
    [FoldoutGroup("Material")] 
    public int itemAmount;
    
    [FoldoutGroup("Currency")]
    public float Gold, Soul;

    [FoldoutGroup("Debug")]
    [ReadOnly] public bool isSkillBuff;
    
    public void Awake()
    {
        isSkillBuff = (Skill != null);
        
        if (Skill != null && itemSprite != null && 
            takeSkillIcon && Skill.TryGetComponent<ISkill>(out ISkill skill))
        {
            itemSprite.sprite = skill.Icon;
        }
    }

    public void GrabItem()
    {
        GrabMaterial();
        GrabMoney();
        GrabSkill();
        ApplyBuff();
    }
    
    public void GrabSkill()
    {
        if(Skill != null) SkillHolder.Instance.AddSkill(Skill);
    }
    public void ApplyBuff()
    {
        if(amount != 0) PlayerController.Instance._stats.BuffPlayer(type, amount);
    }
    public void GrabMoney()
    {
        if (Soul > 0 || Gold > 0) PlayerController.Instance._playerData.AddCurrency(Gold, Soul);
    }

    public void GrabMaterial()
    {
        PlayerController.Instance._playerData.AddItem(itemID, itemAmount);
    }
 }
