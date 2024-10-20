using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMain : MonoBehaviour
{
    //Data
    public UnitType unitType;
    public int Team = 0;
    public Material material;
    public float minRange;
    public float maxRange;
    public float cooldown = 2f;
    public float countdown;
    //Stat
    public BotGun gun;
    //public GameObject projectile;
    public Health Health;
    public CharacterStat MaxHP;
    public CharacterStat HP;
    public CharacterStat Speed;

    public void Start()
    {
        HP = new CharacterStat(20);
    }

    public void ResetCooldown()
    {
        countdown = cooldown;
    }
    public void Dead()
    {
        Destroy(this.gameObject);
    }
}
