using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Buff_Ricochet : ISkill
{
    public float ricochetValue = 1f;
    
    void Start()
    {
        SetStat();
    }
    
    void SetStat()
    {
        //Debug.Log("Add Ricochet!!");
        _pc._stats.bonusRicochetMultiplier = ricochetValue;
        _pc._stats.UpdateStats();
    }

    
}
