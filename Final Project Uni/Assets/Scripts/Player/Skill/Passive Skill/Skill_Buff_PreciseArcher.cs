using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Buff_PreciseArcher : ISkill
{
    void Start()
    {
        SetToggle(true);
    }
    
    void SetToggle(bool toggle)
    {
        _pc._arrowController.IsPreciseArcher = toggle;
    }
}
