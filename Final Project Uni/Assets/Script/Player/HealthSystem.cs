using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : CharacterSystem
{
    public CharacterStat MaxHP = new CharacterStat(10);
    public float HP;
    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP.Value;
    }

    void HPChange(float damage)
    {
        HP += damage;
        CheckDead();
    }
    void CheckDead()
    {
        if (HP <= 0)
        {
            //Dead Logic
        }
    }
}
