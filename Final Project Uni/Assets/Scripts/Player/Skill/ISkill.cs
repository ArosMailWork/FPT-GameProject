using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum SkillType
{
    PASSIVE, ACTIVE
}
public abstract class ISkill : MonoBehaviour
{
    [SerializeField] PlayerController _pc;
    public string Name;
    public float Cooldown;
    public float currentCD;
    public SkillType type;
    // Start is called before the first frame update
    public virtual void Activate()
    {

    }
    void Update()
    {
        Timer();
    }
    public void Assign(PlayerController pc)
    {
        _pc = pc;
    }
    #region Timer
    public void Timer()
    {
        if (currentCD >= 0)
            currentCD -= Time.deltaTime;
    }

    #endregion
}
