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
    public Health Health;
    public CharacterStat Speed;

    public void Dead()
    {
        Destroy(this.gameObject);
    }
}
