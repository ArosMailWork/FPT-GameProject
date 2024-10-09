using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Test_2 : ISkill
{

    public void Awake()
    {
        Name = "test2";
        Cooldown = 2f;
    }
    public override void Activate()
    {
        if (currentCD <= 0)
        {
            Debug.Log(Name);
            currentCD = Cooldown;
        }
    }
}
