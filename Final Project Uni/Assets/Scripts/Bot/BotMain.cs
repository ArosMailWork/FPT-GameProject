using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;

public class BotMain : MonoBehaviour
{
    //Data
    [FoldoutGroup("Setup")]
    public UnitType unitType;
    [FoldoutGroup("Setup")]
    public int Team = 0;
    [FoldoutGroup("Setup")]
    public float minRange, maxRange, MoveAngle = 80f;
    [FoldoutGroup("Setup")]
    public float cooldown = 2f;
    
    [FoldoutGroup("Debug")]
    public float countdown;
    [FoldoutGroup("Debug")]
    public float Distance;
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
