using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Buff_TrapMaster : ISkill
{
    void Start()
    {
        SetToggle(true);
    }
    
    void SetToggle(bool toggle)
    {
        _pc.PlayerHealth.isTrapMaster = toggle;
    }
}
