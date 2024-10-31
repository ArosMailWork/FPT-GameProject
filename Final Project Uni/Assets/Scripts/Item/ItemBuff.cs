using System;
using JetBrains.Annotations;
using PA;
using UnityEngine;
using UnityEngine.UI;

public enum BuffType
{
    Health, Speed, Damage
}

public class ItemBuff : MonoBehaviour
{
    [Header("Base")]
    [CanBeNull] public Image itemSprite;
    
    [Header("Buff")]
    public BuffType type;
    public int amount;
    
    [Space(5)]
    [Header("Skill Only")]
    [Space(5)]
    
    [CanBeNull] public bool takeSkillIcon = true;
    [CanBeNull] public GameObject Skill;

    private void Awake()
    {
        if (Skill != null && itemSprite != null && 
            takeSkillIcon && Skill.TryGetComponent<ISkill>(out ISkill skill))
        {
            itemSprite.sprite = skill.Icon;
        }
    }

    public void GrabSkill()
    {
        if(Skill != null) SkillHolder.Instance.AddSkill(Skill);
    }
    
    public void ApplyBuff()
    {
        if(amount != 0) PlayerController.Instance._stats.BuffPlayer(type, amount);
    }
 }
