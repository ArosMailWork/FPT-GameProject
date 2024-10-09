using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_test_1 : ISkill
{
    public void Awake()
    {
        Name = "test1";
        Cooldown = 2f;
    }
    public override void Activate()
    {
        if (currentCD <= 0)
        {
            Debug.Log(Name);
            currentCD = Cooldown;
            //_pc.GuardDebug();
        }

    }
}
