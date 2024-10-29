using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Active_Splitshot : ISkill
{
    void Start()
    {
        SetToggle(true);
    }
    void SetToggle(bool toggle)
    {
        //Name = "Split shot";
        //type = SkillType.PASSIVE;
        _pc._arrowController.IsSplitShot = toggle;
    }


}
