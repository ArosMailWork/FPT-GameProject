using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Splitshot : ISkill
{
    // Start is called before the first frame update
    void SetToggle(bool toggle)
    {
        Name = "Split shot";
        type = SkillType.PASSIVE;
        _pc.GetComponent<ArrowController>().IsSplitShot = toggle;
    }


}
