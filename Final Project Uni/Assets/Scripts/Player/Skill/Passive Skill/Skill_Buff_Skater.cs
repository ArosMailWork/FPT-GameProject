using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Buff_Skater : ISkill
{
    public float drag = 2f;
    public float bonusSpeed = 4f;
    
    void Start()
    {
        SetStat();
    }

    void SetStat()
    {
        _pc.PlayerRB.drag = drag;
        _pc._stats.bonusSpeed += bonusSpeed;
    }
}
