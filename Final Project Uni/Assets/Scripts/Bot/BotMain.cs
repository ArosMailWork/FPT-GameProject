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
    public Rigidbody rg;
    //Stat
    public BotGun gun;
    //public GameObject projectile;
    public Health Health;

    public void ResetCooldown()
    {
        countdown = cooldown;
    }
    public void Dead()
    {
        Destroy(this.gameObject);
    }
}
