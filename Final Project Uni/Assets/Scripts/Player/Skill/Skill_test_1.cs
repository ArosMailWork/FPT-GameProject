using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_test_1 : ISkill
{
    public new string Name = "test1";
    public new float cooldown = 2f;
    public override void Activate()
    {
        if (currentCD <= 0)
        {
            Debug.Log(Name);
            currentCD = cooldown;
        }

    }
}
