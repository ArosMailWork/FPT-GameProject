using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public class BotMain : MonoBehaviour
{
    //Data
    public UnitType unitType;
    //public int Team = 0;
    
    [FoldoutGroup("Movement Settings")]
    public float minRange, maxRange, MoveAngle = 80f;
    [FoldoutGroup("Movement Settings")]
    public float cooldown = 2f, countdown;

    [FoldoutGroup("Setup")]
    public Rigidbody rg;
    [FoldoutGroup("Setup")]
    public Health Health;
    [FoldoutGroup("Setup")]
    public List<BotGun> gun;
    [FoldoutGroup("Setup")]
    [CanBeNull] public BotAnimController _animController;

    public void ResetCooldown()
    {
        countdown = cooldown;
    }
    public void Dead()
    {
        Destroy(this.gameObject);
    }

    public void Shoot(int gunSlot)
    {
#if !UNITY_EDITOR
        return
#endif
        
        if(gun[gunSlot] != null && gun[gunSlot].target != null) gun[gunSlot].Fire();
    }
}
