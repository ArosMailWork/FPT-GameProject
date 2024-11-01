using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public enum SkillType
{
    PASSIVE, ACTIVE
}
public abstract class ISkill : MonoBehaviour
{
    [FoldoutGroup("Base Skill")] 
    [CanBeNull] public Sprite Icon;
    [FoldoutGroup("Base Skill")]
    [SerializeField] public PlayerController _pc;
    [FoldoutGroup("Base Skill")]
    public string Name;
    [FoldoutGroup("Base Skill")]
    public float Cooldown, currentCD;
    [FoldoutGroup("Base Skill")]
    public SkillType type;
    // Start is called before the first frame update
    public virtual void Activate() { }
    public virtual void Deactivate() { }
    void Update()
    {
        Timer();
    }
    public void Assign(PlayerController pc)
    {
        _pc = pc;
        _pc._playerData.UnlockSkill(Name);
    }
    #region Timer
    public void Timer()
    {
        if (currentCD >= 0)
            currentCD -= Time.deltaTime;
    }

    #endregion
}
