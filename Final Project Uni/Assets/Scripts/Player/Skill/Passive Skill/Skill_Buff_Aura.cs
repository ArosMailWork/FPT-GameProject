using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Buff_Aura : ISkill
{
    void Start()
    {
        SetToggle(true);
    }
    
    void SetToggle(bool toggle)
    {
        if(_pc == null) _pc = PlayerController.Instance;
        _pc.haveAura = toggle;
    }
    
    public override void Deactivate()
    {
        SetToggle(false);
    }
}
