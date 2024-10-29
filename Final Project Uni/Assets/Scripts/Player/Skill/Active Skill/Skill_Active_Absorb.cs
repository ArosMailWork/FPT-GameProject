using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class Skill_Active_Absorb : ISkill
{
    [FoldoutGroup("Stats/Damage")]
    public float AbsorbTime = 5;

    public override void Activate()
    {
        if (currentCD <= 0)
        {
            Debug.Log(Name + " Activated");
            AbsorbBuff();
            currentCD = Cooldown;
        }
    }
    
    [Button]
    public void AbsorbBuff()
    {
        _pc.PlayerHealth.Absorbtion(AbsorbTime);
    }
}
