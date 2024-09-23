using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMain : MonoBehaviour
{
    //Data
    public UnitType unitType;
    public int Team = 0;
    public Material material;
    //Stat
    public CharacterStat MaxHP;
    public CharacterStat HP;
    public CharacterStat Speed;

    public void Start()
    {
        HP = new CharacterStat(20);
    }
    public void Update()
    {
        if (HP.Value <= 0)
        {
            Dead();
        }
    }
    public void Dead()
    {
        Destroy(this.gameObject);
    }
}
